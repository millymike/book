using Book.Api.Dtos;
using Book.Features;
using Book.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

 public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<BookPost>>> GetAllBooks()
    {
        return Ok(await _bookService.GetAllBooks());
    }

    [HttpGet("id")]
    public async Task<ActionResult<BookPost>> GetBookById(int id)
    {
        var book = await _bookService.GetBookById(id);
        if (book == null) return BadRequest("Book not found");
        return Ok(book);
    }

    [HttpGet("id")]
    public async Task<ActionResult<List<BookPost>>> GetBooksByAuthor(int id)
    {
        var books = await _bookService.GetBooksByAuthor(id);
        if (books == null) return BadRequest("Author not found");
        return Ok(books);
    }

    [HttpPost]
    public async Task<ActionResult<BookPost>> AddBook(NewBookDto newBook)
    {
         var author = await _bookService.GetAuthorById(newBook.AuthorId);
        var category = await _bookService.GetCategoryByName(newBook.CategoryName);
        var bookPost = await _bookService.AddBook(new BookPost
        {
            Title = newBook.Title,
            Body = newBook.Body,
            Summary = newBook.Summary,
            Category = category,
            CategoryName = newBook.CategoryName,
            AuthorId = newBook.AuthorId,
            Tags = newBook.Tags,
            BookId = newBook.Id,
            Created = newBook.Created,
            Updated = newBook.Updated,
            Author = author
        });
        if (bookPost == null) return BadRequest("Please add a book in the valid format");
        return Ok(bookPost);
    }

    [HttpPost]
     public async Task<ActionResult<Author?>> AddAuthor(Author newAuthor)
    {
        var author = await _bookService.AddAuthor(newAuthor);
         return Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> AddCategory(Category newCategory)
    {
        var category = await _bookService.AddCategory(newCategory);
        return Ok(category);
    }

    [HttpPut]
    public async Task<ActionResult<BookPost>> UpdateBook(NewBookDto updateBook)
    {
         var author = await _bookService.GetAuthorById(updateBook.Id);
        var category = await _bookService.GetCategoryByName(updateBook.CategoryName);
        var book = await _bookService.GetBookById(updateBook.Id) ?? new BookPost();
        book.Author = author;
        book.Category = category;
        book.Body = updateBook.Body;
        book.Summary = updateBook.Summary;
        book.Tags = updateBook.Tags;
        book.Title = updateBook.Title;
        book.Created = book.Created;
        book.Updated = DateTime.UtcNow;

        var bookPost = await _bookService.UpdateBook(book);
        if (bookPost == null) return BadRequest("Book not found");
        return Ok(bookPost);
    }

    [HttpDelete("id")]
    public ActionResult<Task> DeleteBook(int id)
    {
        return Ok(_bookService.DeleteBook(id));
    }
}