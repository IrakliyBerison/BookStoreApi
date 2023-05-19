using BookStoreApi;
using BookStoreApi.Models;
using BookStoreApi.Repositories;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddTransient<IBookRepository, BookRepository>();
builder.Services.AddTransient<IAuthorRepository, AuthorRepository>();


var app = builder.Build();


app.UseMiddleware<BookStoreLogger>();

using (var scope = app.Services.CreateScope())
{
    using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (dbContext.Database.EnsureCreated())
    {
        List<BookState> bookStates = new List<BookState>()
         {
            new BookState(){ Name="Продана" },
            new BookState(){ Name="В наличии" },
            new BookState(){ Name="Неизвестно" }
        };

        List<Author> authors = new List<Author> {
            new Author() { FirstName = "Михаил", Surname = "Афанасьевич", LastName = "Булгаков", FullName="Булгаков М.И.",  DateOfBirth = new DateTime(1891, 5, 15) },
            new Author() { FirstName = "Федор", Surname = "Михайлович", LastName = "Достоевский", FullName="Достоевский Ф.М.", DateOfBirth = new DateTime(1821, 10, 30) },
            new Author() { FirstName = "Иван", Surname = "Сергеевич", LastName = "Тургенев", FullName="Тургенев B.C.", DateOfBirth = new DateTime(1818, 11, 9) }
        };

        List<Book> books = new List<Book>()
        {
            new Book(){ Author=authors.Where(x=>x.LastName=="Булгаков").FirstOrDefault(), Title="Собачье сердце", CreatedOn=DateTime.Now, ReleaseDate=new DateTime(1925,1,1), State=bookStates.Where(x=>x.Name=="В наличии").FirstOrDefault() },
            new Book(){ Author=authors.Where(x=>x.LastName=="Тургенев").FirstOrDefault(), Title="Отцы и дети", CreatedOn=DateTime.Now, ReleaseDate=new DateTime(1860,1,1), State=bookStates.Where(x=>x.Name=="Продана").FirstOrDefault() },
            new Book(){ Author=authors.Where(x=>x.LastName=="Достоевский").FirstOrDefault(), Title="Село Степанчиково и его обитатели", CreatedOn=DateTime.Now, ReleaseDate=new DateTime(1859,1,1), State=bookStates.Where(x=>x.Name=="Неизвестно").FirstOrDefault() },
        };


        List<User> users = new List<User>()
        {
            new User(){ Login="Manager"},
            new User(){ Login="Salesman"},
            new User(){ Login="Guest"},
        };

        dbContext.BookStates.AddRange(bookStates);
        dbContext.Authors.AddRange(authors);
        dbContext.Books.AddRange(books);
        dbContext.Users.AddRange(users);

        dbContext.SaveChanges();
    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpLogging();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
