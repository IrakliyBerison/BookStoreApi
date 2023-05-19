using BookStoreApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookStoreApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext context;

        public BookRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task DeleteBookAsync(Book book)
        {
            context.Books!.Remove(book);
            await context.SaveChangesAsync();
        }

        public async Task<object> GetAllBooksWithAuthorFullNameAsync()
        {
            var query = from b in context.Books.Include(a => a.Author)
                        select new
                        {
                            b,
                            AuthorFullName = b.Author != null ? b.Author.FullName : "NA"
                        };
            return await query.ToListAsync();
        }

        public async Task<Book> CreateBooksWithAuthorFullNameAsync(string title, string firstName, string lastName, string? surname)
        {
            Author a = new Author()
            {
                FirstName = firstName,
                LastName = lastName,
                Surname = surname
            };
            Book b = new Book()
            {
                Author = a,
                Title = title
            };
            context.Authors.Add(a);
            context.Books.Add(b);

            await context.SaveChangesAsync();

            return b;
        }

        public async Task<List<Book>> GetBookByTitleAsync(string title)
        {
            var query = context.Books.Where(x => x.Title.Contains(title) || title.Contains(x.Title));
            return await query.ToListAsync();
        }

        public async Task<List<Book>> SearchBookByKeyValuesAsync(string value)
        {
            var query = context.Books.Include(a => a.Author).Where(x => x.Title.Contains(value) || value.Contains(x.Title) || x.Author.FirstName.Contains(value) || x.Author.LastName.Contains(value) || x.Author.Surname.Contains(value));
            return await query.ToListAsync();
        }

        public async Task<Book> SetBookStateToExistAsync(Book book)
        {

            book.State = context.BookStates.FirstOrDefault(x => x.Id == 2);
            context.Books.Update(book);

            await context.SaveChangesAsync();

            return book;

        }

        public async Task<Book> SetBookStateToSoldAsync(Book book)
        {
            book.State = context.BookStates.FirstOrDefault(x => x.Id == 1);
            context.Books.Update(book);

            await context.SaveChangesAsync();

            return book;
        }

        public async Task<Book> GetBookByIdAsync(Guid Id)
        {
            return await context.Books.FirstOrDefaultAsync(x => x.Id == Id);
        }

    }
}
