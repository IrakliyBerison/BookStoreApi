using BookStoreApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

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
                await bookRepository.SetBookStateToSoldAsync(book, user);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
