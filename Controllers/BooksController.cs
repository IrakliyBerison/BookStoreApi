using BookStoreApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Net;
using System.Security.Claims;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1);
        private static readonly Dictionary<Guid, TaskCompletionSource<bool>> _recordLocks = new Dictionary<Guid, TaskCompletionSource<bool>>();


        private readonly IBookRepository bookRepository;
        private readonly IUserRepository userRepository;

        public BooksController(IBookRepository bookRepository, IUserRepository userRepository)
        {
            this.bookRepository = bookRepository;
            this.userRepository = userRepository;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("get-all-books")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await bookRepository.GetAllBooksWithAuthorFullNameAsync();
            return Ok(books);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("create-book")]
        public async Task<IActionResult> CreateBooksWithAuthorFullNameAsync(string title, string firstName, string lastName, string? surname)
        {
            var books = await bookRepository.CreateBooksWithAuthorFullNameAsync(title, firstName, lastName, surname);
            return Ok(books);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("get-book-by-title")]
        public async Task<IActionResult> GetBookByTitleAsync(string title)
        {
            var books = await bookRepository.GetBookByTitleAsync(title);
            return Ok(books);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("get-book-by-key-field")]
        public async Task<IActionResult> SearchBookByKeyValuesAsync(string value)
        {
            var books = await bookRepository.SearchBookByKeyValuesAsync(value);
            return Ok(books);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("delete-books")]
        public async Task<IActionResult> DeleteBookAsync(Guid id)
        {
            var book = await bookRepository.GetBookByIdAsync(id);
            await bookRepository.DeleteBookAsync(book);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("set-book-exist")]
        public async Task<IActionResult> SetBookStateToExistAsync(Guid id)
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity && identity.IsAuthenticated)
            {
                var userid = identity.FindFirst("id").Value;
                var user = await userRepository.GetUserAsync(new Guid(userid));
                var book = await bookRepository.GetBookByIdAsync(id);
                if (book == null)
                {
                    return new ObjectResult("Не найдена книга") { StatusCode = (int)HttpStatusCode.NotFound };
                }
                await bookRepository.SetBookStateToExistAsync(book, user);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
        [Authorize]
        [HttpGet]
        [Route("set-book-solded")]
        public async Task<IActionResult> SetBookStateToSoldAsync(Guid id)
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity && identity.IsAuthenticated)
            {
                var userid = identity.FindFirst("id").Value;
                var user = await userRepository.GetUserAsync(new Guid(userid));
                var book = await bookRepository.GetBookByIdAsync(id);
                if (book == null)
                {
                    return new ObjectResult("Не найдена книга") { StatusCode = (int)HttpStatusCode.NotFound };
                }


                if (!_recordLocks.ContainsKey(book.Id))
                {
                    _recordLocks[book.Id] = new TaskCompletionSource<bool>();
                    try
                    {
                        await _lock.WaitAsync();
                        await bookRepository.SetBookStateToSoldAsync(book, user);
                        return Ok("Success");
                    }
                    finally
                    {
                        _lock.Release();
                        _recordLocks[book.Id].TrySetResult(true);
                        _recordLocks.Remove(book.Id);
                    }
                }
                else
                {
                    return new ObjectResult("Данная книга уже переводится в статус продана.") { StatusCode = (int)HttpStatusCode.Conflict };
                }
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
