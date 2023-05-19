using BookStoreApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {

        private readonly IAuthorRepository authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }


        [HttpGet]
        [Route("get-author")]
        public async Task<IActionResult> GetAllAuthorsByNameAsync(string fullName)
        {
            var authors = await authorRepository.GetAllAuthorsByNameAsync(fullName);
            return Ok(authors);
        }



        [HttpGet]
        [Route("create-author")]
        public async Task<IActionResult> CreateAuthorAsync(string firstName, string lastName, string surname)
        {
            var author = await authorRepository.CreateAuthorAsync(firstName, lastName, surname);
            return Ok(author);
        }



        [HttpGet]
        [Route("remove-author")]
        public async Task<IActionResult> RemoveAuthorWithBooksAsync(Guid id)
        {
            var deleted = await authorRepository.RemoveAuthorWithBooksAsync(id);
            if (deleted)
            {
                return Ok(deleted);
            }
            else
            {
                return new ObjectResult("Не найден автор") { StatusCode = (int)HttpStatusCode.NotFound };

            }

        }

    }
}
