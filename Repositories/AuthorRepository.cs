using BookStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BookStoreApi.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext context;

        public AuthorRepository(AppDbContext context)
        {
            this.context = context;
        }


        public async Task<Author> CreateAuthorAsync(string firstName, string lastName, string surname)
        {
            Author author = new Author()
            {
                FirstName = firstName,
                LastName = lastName,
                Surname = surname
            };
            context.Authors.Add(author);
            await context.SaveChangesAsync();

            return author;
        }

        public async Task<List<Author>> GetAllAuthorsByNameAsync(string fullName)
        {
            var query = context.Authors.Where(x => x.FullName.Contains(fullName));

            return await query.ToListAsync();
        }

        public async Task<bool> RemoveAuthorWithBooksAsync(Guid id)
        {
            var author = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if (author != null)
            {
                var books = await context.Books.Where(x => x.Author != null && x.Author.Id == author.Id).ToListAsync();
                context.Books.RemoveRange(books);
                context.Authors.Remove(author);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
