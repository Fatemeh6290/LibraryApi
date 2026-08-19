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
    public ActionResult<List<Book>> GetBooks()
    {
        return _bookService.GetBooks();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetBookById(int id)
    {
        var book = _bookService.GetBookById(id);
        
        if (book == null)
            return NotFound();

        return book;
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var result = _bookService.DeleteBook(id);
        
        if (!result)
            return NotFound();
        
        return NoContent();
    }
}