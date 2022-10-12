using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Book.Models;
using Book.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Book.Features;

public class BookService : IBookService
{
    private readonly AppSettings _appSettings;
    private readonly DataContext _dataContext;

    public BookService(DataContext dataContext, AppSettings appSettings)
    {
        _dataContext = dataContext;
        _appSettings = appSettings;
    }

    public async Task<List<BookPost>> GetAllBooks()
    {
        return await _dataContext.BookPosts.Include(x => x.Author)
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task<BookPost?> GetBookById(int id)
    {
        var book = await _dataContext.BookPosts.Where(x => x.BookId == id)
            .Include(x => x.Author)
            .Include(x => x.Category)
            .FirstOrDefaultAsync();

        if (book == null) return null;

        return book;
    }

    public async Task<List<BookPost>> GetBooksByAuthor(int id)
    {
        var books = await _dataContext.BookPosts.Where(x => x.AuthorId == id)
            .Include(x => x.Author)
            .Include(x => x.Category)
            .ToListAsync();

        return books;
    }


    public async Task<BookPost?> AddBook(BookPost newBook)
    {
        var author = await _dataContext.Authors.Where(x => x.AuthorId == newBook.AuthorId)
            .Include(x => x.BookPosts)
            .FirstOrDefaultAsync();

        var category = await _dataContext.Categories
            .Where(x => x.CategoryName.ToUpper() == newBook.CategoryName.ToUpper())
            .FirstOrDefaultAsync();

        var book = new BookPost
        {
            Title = newBook.Title,
            Summary = newBook.Summary,
            Body = newBook.Body,
            Author = author,
            Tags = newBook.Tags,
            AuthorId = newBook.AuthorId,
            Category = category,
            CategoryName = newBook.CategoryName,
            Updated = newBook.Updated,
            Created = DateTime.UtcNow
        };
        _dataContext.BookPosts.Add(book);
        await _dataContext.SaveChangesAsync();
        return book;
    }

    public async Task<BookPost> UpdateBook(BookPost updateBook)
    {
        var author = await _dataContext.Authors.Where(x => x.AuthorId == updateBook.AuthorId)
            .Include(x => x.BookPosts)
            .FirstOrDefaultAsync();
        var category = await _dataContext.Categories
            .Where(x => x.CategoryName.ToUpper() == updateBook.CategoryName.ToUpper())
            .FirstOrDefaultAsync();
        if (author == null) return null;
        var book = await _dataContext.BookPosts.FindAsync(updateBook.BookId);
        book.BookId = updateBook.BookId;
        book.Title = updateBook.Title;
        book.Summary = updateBook.Summary;
        book.Body = updateBook.Body;
        book.Tags = updateBook.Tags;
        book.Category = category;
        book.Author = author;
        book.Created = book.Created;
        book.Updated = DateTime.UtcNow;

        _dataContext.BookPosts.Update(book);
        await _dataContext.SaveChangesAsync();
        return book;
    }

    public async Task<Author> AddAuthor(Author newAuthor)
    {
        var book = await _dataContext.BookPosts.Where(x => x.AuthorId == newAuthor.AuthorId)
            .Include(x => x.Author)
            .ToListAsync();
        var author = new Author
        {
            AuthorId = newAuthor.AuthorId,
            Name = newAuthor.Name,
            Description = newAuthor.Description,
            BookPosts = book
        };
        _dataContext.Authors.Add(author);
        await _dataContext.SaveChangesAsync();
        return author;
    }

    public async Task<Category> AddCategory(Category newCategory)
    {
        var category = new Category
        {
            CategoryName = newCategory.CategoryName
        };
        _dataContext.Categories.Add(category);
        await _dataContext.SaveChangesAsync();
        return category;
    }

    public async Task DeleteBook(int id)
    {
        var book = await _dataContext.BookPosts.FindAsync(id);
        _dataContext.BookPosts.Remove(book);
        await _dataContext.SaveChangesAsync();
    }

     public async Task<User?> GetUserByEmailAddress(string emailAddress)
    {
        return await _dataContext.Users.SingleOrDefaultAsync(u => u!.EmailAddress == emailAddress);
    }

    public async Task<Author?> GetAuthorById(int authorId)
    {
        return await _dataContext.Authors.SingleOrDefaultAsync(a => a.AuthorId == authorId);
    }

    public async Task<Category?> GetCategoryByName(string categoryName)
    {
        return await _dataContext.Categories
            .SingleOrDefaultAsync(x => x.CategoryName.ToLower() == categoryName.ToLower());
    }

    public async Task<User> CreateUser(User user)
    {
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();
        return user;
    }

    public async Task<string?> CreatePasswordHash(string password)
    {
        return await Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
    }


    public bool VerifyPassword(string password, User user)
    {
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public async Task<string?> CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_appSettings.JwtSecret));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken
        (
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}
