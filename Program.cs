using BookStoreApi;
using BookStoreApi.Models;
using BookStoreApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IBookRepository, BookRepository>();
builder.Services.AddTransient<IAuthorRepository, AuthorRepository>();



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();


app.UseMiddleware<BookStoreLogger>();

using (var scope = app.Services.CreateScope())
{
    using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (dbContext.Database.EnsureCreated())
    {
        List<BookState> bookStates = new List<BookState>()
         {
            new BookState(){ Name="�������" },
            new BookState(){ Name="� �������" },
            new BookState(){ Name="����������" }
        };

        List<Author> authors = new List<Author> {
            new Author() { FirstName = "������", Surname = "�����������", LastName = "��������", FullName="�������� �.�.",  DateOfBirth = new DateTime(1891, 5, 15) },
            new Author() { FirstName = "�����", Surname = "����������", LastName = "�����������", FullName="����������� �.�.", DateOfBirth = new DateTime(1821, 10, 30) },
            new Author() { FirstName = "����", Surname = "���������", LastName = "��������", FullName="�������� B.C.", DateOfBirth = new DateTime(1818, 11, 9) }
        };

        List<Book> books = new List<Book>()
        {
            new Book(){ Author=authors.Where(x=>x.LastName=="��������").FirstOrDefault(), Title="������� ������", CreatedOn=DateTime.Now, ReleaseDate=new DateTime(1925,1,1), State=bookStates.Where(x=>x.Name=="� �������").FirstOrDefault() },
            new Book(){ Author=authors.Where(x=>x.LastName=="��������").FirstOrDefault(), Title="���� � ����", CreatedOn=DateTime.Now, ReleaseDate=new DateTime(1860,1,1), State=bookStates.Where(x=>x.Name=="�������").FirstOrDefault() },
            new Book(){ Author=authors.Where(x=>x.LastName=="�����������").FirstOrDefault(), Title="���� ������������ � ��� ���������", CreatedOn=DateTime.Now, ReleaseDate=new DateTime(1859,1,1), State=bookStates.Where(x=>x.Name=="����������").FirstOrDefault() },
        };


        List<User> users = new List<User>()
        {
            new User(){ Email="Manager@bookstore.book", Password="AllYouCanGet", UserName="Benjamin"}
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
