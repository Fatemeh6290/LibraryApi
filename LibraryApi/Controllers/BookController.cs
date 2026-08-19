using LibraryApi.DTOs;
using LibraryApi.Interfaces;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    
    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }
    
    [HttpGet]
    public ActionResult<List<BookDto>> GetBooks()
    {
        var books = _bookService.GetBooks();
        
        var result = books.Select(book => new BookDto
        {
            BookId = book.BookId,
            Author = book.Author,
            Title = book.Title,
            IsAvailable = book.IsAvailable,
            PublishedYear = book.PublishedYear,
        }).ToList();

        return result;
    }

    [HttpGet("{id}")]
    public ActionResult<BookDto> GetBookById(int id)
    {
        var book = _bookService.GetBookById(id);
        
        if (book == null)
            return NotFound();

        var result = new BookDto
        {
            BookId = book.BookId,
            Author = book.Author,
            Title = book.Title,
            IsAvailable = book.IsAvailable,
            PublishedYear = book.PublishedYear
        };

        return result;
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var result = _bookService.DeleteBook(id);
        
        if (!result)
            return NotFound();
        
        return NoContent();
    }

    [HttpPost]
    public IActionResult AddBook(CreateBookDto dto)
    {
        var result = _bookService.AddBook(
            dto.Title,
            dto.Author,
            dto.PublishedYear);
        
        if (!result)
            return BadRequest("A book with the same title already exists.");
        
        return Ok();
    }
}