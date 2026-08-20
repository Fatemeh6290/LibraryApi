using LibraryApi.Interfaces;
using LibraryApi.Models;

namespace LibraryApi.Services;

public class BookService: IBookService
{
    private readonly List<Book> _books = new();
    private readonly ILogger<BookService> _logger;
    private int _bookId = 1;

    public BookService(ILogger<BookService> logger)
    {
        _logger = logger;
    }
    
    public List<Book> GetBooks()
    {
        return _books.ToList();
    }

    public Book? GetBookById(int id)
    {
        return _books.FirstOrDefault(x => x.BookId == id);
    }

    public Book? AddBook(string title, string author, int publishedYear)
    {
        if (_books.Any(x => x.Title == title))
        {
            _logger.LogWarning("A book with the same title already exists.");
            return null;
        }
        
        var book = new Book
        {
            BookId = _bookId++,
            Title = title,
            Author = author,
            PublishedYear = publishedYear,
            IsAvailable = true
        };
        
        _books.Add(book);
        _logger.LogInformation("Book with Id {BookId} added.", book.BookId);
        
        return book;
    }

    public bool DeleteBook(int id)
    {
        var book = _books.FirstOrDefault(x => x.BookId == id);

        if (book is null)
        {
            _logger.LogWarning("Book with Id {BookId} not found.", id);
            return false;
        }
        
        _books.Remove(book);
        _logger.LogInformation("Book with Id {BookId} deleted.", book.BookId);
        
        return true;
    }
}