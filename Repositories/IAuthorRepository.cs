using BookStoreApi.Models;

namespace BookStoreApi.Repositories
{
    public interface IAuthorRepository
    {
        public Task<List<Author>> GetAllAuthorsByNameAsync(string fullName);
        public Task<Author> CreateAuthorAsync(string firstName, string lastName, string surname);
        public Task<bool> RemoveAuthorWithBooksAsync(Guid id);
    }
}
