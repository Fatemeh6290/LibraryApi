using LibraryApi.Data;
using LibraryApi.Interfaces;
using LibraryApi.Models;

namespace LibraryApi.Services;

public class BookService: IBookService
{
    private readonly LibraryDbContext _context;
    private readonly ILogger<BookService> _logger;

    public BookService(LibraryDbContext context, ILogger<BookService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public List<Book> GetBooks()
    {
        return _context.Books.ToList();
    }

    public Book? GetBookById(int id)
    {
        return _context.Books.FirstOrDefault(x => x.BookId == id);
    }

    public Book? AddBook(string title, string author, int publishedYear)
    {
        if (_context.Books.Any(x => x.Title == title))
        {
            _logger.LogWarning("A book with the same title already exists.");
            return null;
        }
        
        var book = new Book
        {
            Title = title,
            Author = author,
            PublishedYear = publishedYear,
            IsAvailable = true
        };
        
        _context.Books.Add(book);
        _context.SaveChanges();
        
        _logger.LogInformation("Book with Id {BookId} added.", book.BookId);
        
        return book;
    }

    public bool DeleteBook(int id)
    {
        var book = _context.Books.FirstOrDefault(x => x.BookId == id);

        if (book is null)
        {
            _logger.LogWarning("Book with Id {BookId} not found.", id);
            return false;
        }
        
        _context.Books.Remove(book);
        _context.SaveChanges();
        
        _logger.LogInformation("Book with Id {BookId} deleted.", book.BookId);
        
        return true;
    }
}