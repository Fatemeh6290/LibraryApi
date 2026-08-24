using System.Net;
using System.Net.Http.Json;
using LibraryApi.DTOs;

namespace LibraryApiTests;

public class BookControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BookControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetBooks_ShouldReturnSuccess()
    {
        //Act
        var response = await _client.GetAsync("/api/Book");

        //Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, content);
    }
    
    [Fact]
    public async Task GetBooks_ShouldReturnBooks()
    {
        //Act
        var response = await _client.GetAsync("/api/Book");
        
        //Assert
        var book = await response.Content.ReadFromJsonAsync<List<BookDto>>();
        Assert.NotNull(book);
        Assert.NotEmpty(book);
    }
    
    [Fact]
    public async Task CreateBook_ShouldReturnCreatedBook()
    {
        // Arrange
        var request = new
        {
            Title = "Clean Code",
            Author = "Martin Joe",
            PublishedYear = 2020
        };

        // Act
        var createdResponse =
            await _client.PostAsJsonAsync("/api/Book", request);

        var content =
            await createdResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.True(
            createdResponse.IsSuccessStatusCode,content);

        var createdBook =
            await createdResponse.Content.ReadFromJsonAsync<BookDto>();

        Assert.NotNull(createdBook);
        Assert.Equal(request.Title, createdBook.Title);
        Assert.Equal(request.Author, createdBook.Author);
        Assert.Equal(request.PublishedYear, createdBook.PublishedYear);
    }
    
    [Fact]
    public async Task GetBookById_ShouldReturnBook()
    {
        //Arrange
        var request = new CreateBookDto
        {
            Title = "Harry Potter",
            Author = "Martin Joe",
            PublishedYear = 2020
        };
        
        var createResponse = await _client.PostAsJsonAsync("/api/Book", request);
        var createdBook = await createResponse.Content.ReadFromJsonAsync<BookDto>();
        
        //Act
        var response = await _client.GetAsync($"/api/Book/{createdBook!.BookId}");
        
        //Assert
        var book = await response.Content.ReadFromJsonAsync<BookDto>();

        Assert.NotNull(book);
        Assert.Equal(createdBook.BookId, book.BookId);
        Assert.Equal("Harry Potter", book.Title);
        Assert.Equal("Martin Joe", book.Author);
        Assert.Equal(2020, book.PublishedYear);
    }
    
    [Fact]
    public async Task GetBookById_ShouldReturnNotFound()
    {
        //Act
        var response = await _client.GetAsync("/api/Book/9999");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound ,response.StatusCode);
    }
    
    
    [Fact]
    public async Task CreateBook_ShouldReturnBadRequest_WhenInvalidData()
    {
        //Arrange
        var request = new
        {
            Title = "",
            Author = "Martin Joe",
            PublishedYear = 2020
        };
        
        //Act
        var response = await _client.PostAsJsonAsync("/api/Book", request);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateBook_ShouldReturnBadRequest_WhenDuplicateTitle()
    {
        //Arrange
        var request1 = new
        {
            Title = "Harry Potter",
            Author = "Martin Joe",
            PublishedYear = 2020
        };
        
        var request2 = new
        {
            Title = "Harry Potter",
            Author = "Martin Joe",
            PublishedYear = 2020
        };
        
        //Act
        await _client.PostAsJsonAsync("/api/Book", request1);
        var response = await _client.PostAsJsonAsync("/api/Book", request2);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteBook_ShouldReturnSuccess()
    {
        //Arrange
        var request = new
        {
            Title = "Harry Potter",
            Author = "Martin Joe",
            PublishedYear = 2020
        };
        
        //Act
        var createdResponse = await _client.PostAsJsonAsync("/api/Book", request);
        var book = await createdResponse.Content.ReadFromJsonAsync<BookDto>();
        
        //Assert
        var deleteResponse = await _client.DeleteAsync($"/api/Book/{book!.BookId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
    
    [Fact]
    public async Task DeleteBook_ShouldReturnNotFound()
    {
        //Act
        var deleteResponse = await _client.DeleteAsync($"/api/Book/{1}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
    }
}