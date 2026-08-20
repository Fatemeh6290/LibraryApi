using LibraryApi.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace LibraryApiTests;

public class BookServiceTests
{
    [Fact]
    public void AddBook_ShouldAddBook()
    {
        //Arrange
        BookService bookservice = new BookService(NullLogger<BookService>.Instance);
        
        //Act
        bookservice.AddBook("Harry Potter", "Tom", 1997);
        var result = bookservice.GetBooks();
        
        //Assert
        Assert.Single(result);
        Assert.NotNull(result);
    }
    [Fact]
    public void AddBook_ShouldReturnFalse_WhenTitleAlreadyExists()
    {
        //Arrange
        BookService bookservice = new BookService(NullLogger<BookService>.Instance);
        
        //Act
        bookservice.AddBook("Harry Potter", "Tom", 1997);
        var result = bookservice.AddBook("Harry Potter", "Tom", 1997);
        
        //Assert
        Assert.Null(result);
    }
    [Fact]
    public void GetBooks_ShouldReturnAllBooks()
    {
        //Arrange
        BookService bookservice = new BookService(NullLogger<BookService>.Instance);
        
        //Act
        bookservice.AddBook("Harry Potter", "Tom", 1997); 
        bookservice.AddBook("UML", "Tom", 2012);
        var result = bookservice.GetBooks();
        
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
        BookService bookservice = new BookService(NullLogger<BookService>.Instance);
        
        //Act
        bookservice.AddBook("Harry Potter", "Tom", 1997); 
        bookservice.AddBook("UML", "Tom", 2012);
        var result1 = bookservice.GetBookById(1);
        var result2 = bookservice.GetBookById(2);
        
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
        BookService bookservice = new BookService(NullLogger<BookService>.Instance);
        
        //Act
        bookservice.AddBook("Harry Potter", "Tom", 1997); 
        var result = bookservice.GetBookById(2);
        
        //Assert
        Assert.Null(result);
    }
    [Fact]
    public void DeleteBook_ShouldDeleteBook()
    {
        //Arrange
        BookService bookservice = new BookService(NullLogger<BookService>.Instance);
        
        //Act
        bookservice.AddBook("Harry Potter", "Tom", 1997); 
        var result = bookservice.DeleteBook(1);
        
        //Assert
        Assert.True(result);
        Assert.Empty(bookservice.GetBooks());
    }
    [Fact]
    public void DeleteBook_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        //Arrange
        BookService bookservice = new BookService(NullLogger<BookService>.Instance);
        
        //Act
        bookservice.AddBook("Harry Potter", "Tom", 1997); 
        var result = bookservice.DeleteBook(2);
        
        //Assert
        Assert.False(result);
        Assert.NotNull(bookservice.GetBookById(1));
    }
}