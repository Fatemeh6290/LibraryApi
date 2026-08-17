using System.Runtime.InteropServices.JavaScript;
using LibraryApi.Interfaces;
using LibraryApi.Models;

namespace LibraryApi.Services;

public class BookService: IBookService
{
    private readonly List<Book> _books = new();
    private int _bookId = 1;
    public List<Book> GetBooks()
    {
        return _books.ToList();
    }

    public Book? GetBookById(int id)
    {
        return _books.FirstOrDefault(x => x.BookId == id);
    }

    public bool AddBook(string title, string author, int publishedYear)
    {
        if (_books.Any(x => x.Title == title))
            return false;
        
        _books.Add(new Book
        {
            BookId = _bookId++,
            Title = title,
            Author = author,
            PublishedYear = publishedYear,
            IsAvailable = true
        });
        return true;
    }

    public bool DeleteBook(int id)
    {
        var book = _books.FirstOrDefault(x => x.BookId == id);
        
        if (book is null)
            return false;
        
        _books.Remove(book);
        return true;
    }
}