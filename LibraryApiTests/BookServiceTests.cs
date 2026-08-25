using LibraryApi.Data;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace LibraryApiTests;

public class BookServiceTests
{
    private readonly LibraryDbContext _context;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _context = new LibraryDbContext(options);
        _bookService = new BookService(_context, NullLogger<BookService>.Instance);
    }
    
    [Fact]
    public void AddBook_ShouldAddBook()
    {
        //Act
        var result = _bookService.AddBook("Harry Potter", "Tom", 1997);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal("Harry Potter", result.Title);
        Assert.Equal("Tom", result.Author);
        Assert.Equal(1997, result.PublishedYear);
        Assert.True(result.IsAvailable);
    }
    
    [Fact]
    public void AddBook_ShouldReturnNull_WhenTitleAlreadyExists()
    {
        //Arrange
        _bookService.AddBook("Harry Potter", "Tom", 1997);
        
        //Act
        var result = _bookService.AddBook("Harry Potter", "Tom", 1997);
        
        //Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void GetBooks_ShouldReturnAllBooks()
    {
        //Arrange
        _bookService.AddBook("Harry Potter", "Tom", 1997); 
        _bookService.AddBook("UML", "Tom", 2012);
        
        //Act
        var result = _bookService.GetBooks();
        
        //Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Harry Potter", result[0].Title);
        Assert.Equal("Tom", result[0].Author);
        Assert.Equal(1997, result[0].PublishedYear);
        Assert.True(result[0].IsAvailable);    
    }
    
    [Fact]
    public void GetBookById_ShouldReturnBook()
    {
        //Arrange
        _bookService.AddBook("Harry Potter", "Tom", 1997); 
        _bookService.AddBook("UML", "Tom", 2012);
        
        //Act
        var result1 = _bookService.GetBookById(1);
        var result2 = _bookService.GetBookById(2);
        
        //Assert
        Assert.NotNull(result1);
        Assert.Equal("Harry Potter", result1.Title);
        Assert.NotNull(result2);
        Assert.Equal("UML", result2.Title);
    }
    
    [Fact]
    public void GetBookById_ShouldReturnNull_WhenBookDoesNotExist()
    {
        //Arrange
        _bookService.AddBook("Harry Potter", "Tom", 1997); 
        
        //Act
        var result = _bookService.GetBookById(2);
        
        //Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void DeleteBook_ShouldDeleteBook()
    {
        //Arrange
        _bookService.AddBook("Harry Potter", "Tom", 1997); 
        
        //Act
        var result = _bookService.DeleteBook(1);
        
        //Assert
        Assert.True(result);
        Assert.Empty(_bookService.GetBooks());
    }
    
    [Fact]
    public void DeleteBook_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        //Arrange
        _bookService.AddBook("Harry Potter", "Tom", 1997); 
        
        //Act
        var result = _bookService.DeleteBook(2);
        
        //Assert
        Assert.False(result);
        Assert.NotNull(_bookService.GetBookById(1));
    }
}