using BookStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
              .Property(u => u.FullName)
              .HasComputedColumnSql("[FirstName] + ' ' + [Surname] + ' ' + [LastName]");
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookState> BookStates { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
