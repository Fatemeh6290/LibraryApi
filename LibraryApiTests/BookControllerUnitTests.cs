using LibraryApi.Controllers;
using LibraryApi.DTOs;
using LibraryApi.Interfaces;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApiTests;

public class BookControllerUnitTests
{
    [Fact]
    public void GetBooks_ShouldReturnAllBooks()
    {
        //Arrange
        var books = new List<Book>
        {
            new Book
            {
                BookId = 1,
                Title = "Harry Potter",
                Author = "John Doe",
                PublishedYear = 2000,
                IsAvailable = true
            }
        };
        
        var mockBookService = new Mock<IBookService>();
        mockBookService.Setup(x => x.GetBooks()).Returns(books);
        var controller = new BookController(mockBookService.Object);
        
        //Act
        var result = controller.GetBooks();
        
        //Assert
        Assert.NotNull(result.Value);
        Assert.Single(result.Value);
        Assert.Equal(1, result.Value[0].BookId);
        Assert.Equal("Harry Potter", result.Value[0].Title);
        Assert.Equal("John Doe", result.Value[0].Author);
        Assert.Equal(2000, result.Value[0].PublishedYear);
        Assert.True(result.Value[0].IsAvailable);
        
        mockBookService.Verify(x => x.GetBooks(), Times.Once);
    }
    
    [Fact]
    public void GetBookById_ShouldReturnBook()
    {
        //Arrange
        var books = new List<Book>
        {
            new Book
            {
                BookId = 1,
                Title = "Harry Potter",
                Author = "John Doe",
                PublishedYear = 2000,
                IsAvailable = true
            }
        };
        
        var mockBookService = new Mock<IBookService>();
        mockBookService.Setup(x => x.GetBookById(1)).Returns(books[0]);
        var controller = new BookController(mockBookService.Object);
        
        //Act
        var result = controller.GetBookById(1);
        
        //Assert
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.BookId);
        
        Assert.Equal("Harry Potter", result.Value.Title);
        Assert.Equal("John Doe", result.Value.Author);
        Assert.Equal(2000, result.Value.PublishedYear);
        Assert.True(result.Value.IsAvailable);
        
        mockBookService.Verify(x => x.GetBookById(1), Times.Once);
    }
    
    [Fact]
    public void GetBookById_ShouldReturnNotFound()
    {
        //Arrange
        var mockBookService = new Mock<IBookService>();
        mockBookService.Setup(x => x.GetBookById(9999)).Returns((Book?)null);
        var controller = new BookController(mockBookService.Object);
        
        //Act
        var result = controller.GetBookById(9999);
        
        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
        
        mockBookService.Verify(x => x.GetBookById(9999), Times.Once);
    }
    
    [Fact]
    public void AddBook_ShouldReturnCreatedBook()
    {
        //Arrange
        var mockBookService = new Mock<IBookService>();
        mockBookService.Setup(x => x.AddBook("Harry Potter", "John Doe", 2000))
            .Returns(
                new Book
                {
                    BookId = 1,Title = "Harry Potter", 
                    Author = "John Doe", 
                    PublishedYear = 2000, 
                    IsAvailable = true
                });
        var controller = new BookController(mockBookService.Object);
        var request = new CreateBookDto
        {
            Title = "Harry Potter",
            Author = "John Doe",
            PublishedYear = 2000
        };
        
        //Act
        var result = controller.AddBook(request);
        
        //Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdBook = Assert.IsType<BookDto>(createdResult.Value);
        
        Assert.NotNull(createdBook);
        Assert.Equal(1, createdBook.BookId);
        Assert.Equal("Harry Potter", createdBook.Title);
        Assert.Equal("John Doe", createdBook.Author);
        Assert.Equal(2000, createdBook.PublishedYear);
        Assert.True(createdBook.IsAvailable);
        
        mockBookService.Verify(x => x.AddBook("Harry Potter", "John Doe", 2000), Times.Once);
    }
    
    [Fact]
    public void AddBook_ShouldReturnBadRequest()
    {
        //Arrange
        var mockBookService = new Mock<IBookService>();
        mockBookService.Setup(x => x.AddBook("Harry Potter", "John Doe", 2000)).Returns((Book?)null);
        var controller = new BookController(mockBookService.Object);
        var request = new CreateBookDto
        {
            Title = "Harry Potter",
            Author = "John Doe",
            PublishedYear = 2000
        };
        
        //Act
        var result = controller.AddBook(request);
        
        //Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        
        mockBookService.Verify(x => x.AddBook("Harry Potter", "John Doe", 2000), Times.Once);
    }
    
    [Fact]
    public void DeleteBook_ShouldReturnNoContent()
    {
        var mockBookService = new Mock<IBookService>();
        mockBookService.Setup(x => x.DeleteBook(1)).Returns(true);
        var controller = new BookController(mockBookService.Object);
        
        //Act
        var result = controller.DeleteBook(1);
        
        //Assert
        Assert.IsType<NoContentResult>(result);
        
        mockBookService.Verify(x => x.DeleteBook(1), Times.Once);
    }
    
    [Fact]
    public void DeleteBook_ShouldReturnNotFound()
    {
        var mockBookService = new Mock<IBookService>();
        mockBookService.Setup(x => x.DeleteBook(9999)).Returns(false);
        var controller = new BookController(mockBookService.Object);
        
        //Act
        var result = controller.DeleteBook(9999);
        
        //Assert
        Assert.IsType<NotFoundResult>(result);
        
        mockBookService.Verify(x => x.DeleteBook(9999), Times.Once);

    }
}