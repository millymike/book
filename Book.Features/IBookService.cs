using Book.Models;

namespace Book.Features;

public interface IBookService
{
    public Task<List<BookPost>> GetAllBooks();
    public Task<BookPost?> GetBookById(int id);
    public Task<List<BookPost>> GetBooksByAuthor(int id);
    public Task<BookPost?> AddBook(BookPost newBook);
    public Task DeleteBook(int id);
    public Task<BookPost?> UpdateBook(BookPost updateBook);
    public Task<Author> AddAuthor(Author newAuthor);
    public Task<Category> AddCategory(Category newCategory);
    public Task<User?> GetUserByEmailAddress(string userName);
    public Task<Author?> GetAuthorById(int authorId);
    public Task<Category?> GetCategoryByName(string categoryName);
    public Task<User> CreateUser(User user);
    public Task<string?> CreatePasswordHash(string password);
    public bool VerifyPassword(string password, User user);
    public Task<string?> CreateToken(User user);
   
}