using BookStoreApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly IBookRepository bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        [HttpGet]
        [Route("get-all-books")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await bookRepository.GetAllBooksWithAuthorFullNameAsync();
            return Ok(books);
        }

        [HttpGet]
        [Route("create-book")]
        public async Task<IActionResult> CreateBooksWithAuthorFullNameAsync(string title, string firstName, string lastName, string? surname)
        {
            var books = await bookRepository.CreateBooksWithAuthorFullNameAsync(title, firstName, lastName, surname);
            return Ok(books);
        }

        [HttpGet]
        [Route("get-book-by-title")]
        public async Task<IActionResult> GetBookByTitleAsync(string title)
        {
            var books = await bookRepository.GetBookByTitleAsync(title);
            return Ok(books);
        }


        [HttpGet]
        [Route("get-book-by-key-field")]
        public async Task<IActionResult> SearchBookByKeyValuesAsync(string value)
        {
            var books = await bookRepository.SearchBookByKeyValuesAsync(value);
            return Ok(books);
        }


        [HttpGet]
        [Route("delete-books")]
        public async Task<IActionResult> DeleteBookAsync(Guid id)
        {
            var book = await bookRepository.GetBookByIdAsync(id);
            await bookRepository.DeleteBookAsync(book);
            return Ok();
        }


        [HttpGet]
        [Route("set-book-exist")]
        public async Task<IActionResult> SetBookStateToExistAsync(Guid id)
        {
            var book = await bookRepository.GetBookByIdAsync(id);
            await bookRepository.SetBookStateToExistAsync(book);
            return Ok();
        }

        [HttpGet]
        [Route("set-book-solded")]
        public async Task<IActionResult> SetBookStateToSoldAsync(Guid id)
        {
            var book = await bookRepository.GetBookByIdAsync(id);
            await bookRepository.SetBookStateToSoldAsync(book);
            return Ok();
        }

    }
}
