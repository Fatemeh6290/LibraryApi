using LibraryApi.DTOs;
using LibraryApi.Interfaces;
using LibraryApi.Mapper;
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
        
        var result = books.Select(BookMapper.ToDto).ToList();

        return result;
    }

    [HttpGet("{id}")]
    public ActionResult<BookDto> GetBookById(int id)
    {
        var book = _bookService.GetBookById(id);
        
        if (book == null)
            return NotFound();

        var result = BookMapper.ToDto(book);

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
    public ActionResult<BookDto> AddBook(CreateBookDto dto)
    {
        var book = _bookService.AddBook(
            dto.Title,
            dto.Author,
            dto.PublishedYear);
        
        if (book is null)
            return BadRequest("A book with the same title already exists.");

        var result = BookMapper.ToDto(book);
        
        return CreatedAtAction(
            nameof(GetBookById),
            new { id = book.BookId },
            result);
    }
    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Test exception");
    }
}