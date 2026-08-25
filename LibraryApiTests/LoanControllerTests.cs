using System.Net;
using System.Net.Http.Json;
using LibraryApi.DTOs;

namespace LibraryApiTests;

public class LoanControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public LoanControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetLoans_ShouldReturnSuccess()
    {
        //Act
        var response = await _client.GetAsync("/api/loan");
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetLoans_ShouldReturnLoans()
    {
        //Arrange
        var bookRequest = new CreateBookDto
        {
            Title = "UML",
            Author = "Robert Martin",
            PublishedYear = 2008
        };

        var bookResponse = await _client.PostAsJsonAsync("/api/book", bookRequest);
        var book = await bookResponse.Content.ReadFromJsonAsync<BookDto>();
        
        var memberRequest = new CreateMemberDto
        {
            Name = "Peter Martin",
            Email = "peter@example.com"
        };

        //Act
        var memberResponse = await _client.PostAsJsonAsync("/api/member", memberRequest);
        var member = await memberResponse.Content.ReadFromJsonAsync<MemberDto>();
        
        var loanRequest = new CreateLoanDto
        {
            BookId = book!.BookId,
            MemberId = member!.MemberId
        };
        
        await _client.PostAsJsonAsync("/api/loan", loanRequest);
        
        //Assert
        var response = await _client.GetAsync("/api/loan");
        var loans = await response.Content.ReadFromJsonAsync<List<LoanDto>>();
        Assert.NotNull(loans);
        Assert.Contains(loans, x => x.BookId == book.BookId && x.MemberId == member.MemberId);
    }

    [Fact]
    public async Task GetLoanById_ShouldReturnLoan()
    {
        //Arrange
        var bookRequest = new CreateBookDto
        {
            Title = "Harry Potter",
            Author = "Robert Martin",
            PublishedYear = 2008
        };

        var bookResponse = await _client.PostAsJsonAsync("/api/book", bookRequest);
        var book = await bookResponse.Content.ReadFromJsonAsync<BookDto>();
        
        var memberRequest = new CreateMemberDto
        {
            Name = "Tim Joe",
            Email = "tim@example.com"
        };

        var memberResponse = await _client.PostAsJsonAsync("/api/member", memberRequest);
        var member = await memberResponse.Content.ReadFromJsonAsync<MemberDto>();
        
        var loanRequest = new CreateLoanDto
        {
            BookId = book!.BookId,
            MemberId = member!.MemberId
        };
        
        var loanResponse = await _client.PostAsJsonAsync("/api/loan", loanRequest);
        var createdLoan = await loanResponse.Content.ReadFromJsonAsync<LoanDto>();
        
        //Act
        var response = await _client.GetAsync($"/api/loan/{createdLoan!.LoanId}");
        var loan = await response.Content.ReadFromJsonAsync<LoanDto>();
        
        //Assert
        Assert.NotNull(loan);
        Assert.Equal(book.BookId, loan.BookId);
        Assert.Equal(member.MemberId, loan.MemberId);
    }
    
    [Fact]
    public async Task GetLoanById_ShouldReturnNotFound()
    {
        //Act
        var response = await _client.GetAsync("/api/loan/99999");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateLoan_ShouldReturnCreatedLoan()
    {
        //Arrange
        var requestBook = new CreateBookDto
        {
            Title = "SQL",
            Author = "Robert Martin",
            PublishedYear = 2008
        };
        
        var responseBook = await _client.PostAsJsonAsync("/api/book", requestBook);
        var book = await responseBook.Content.ReadFromJsonAsync<BookDto>();

        var requestMember = new CreateMemberDto
        {
            Name = "Simon Martin",
            Email = "simon@example.com"
        };
        
        var responseMember = await _client.PostAsJsonAsync("/api/member", requestMember);
        var member = await responseMember.Content.ReadFromJsonAsync<MemberDto>();

        var requestLoan = new CreateLoanDto
        {
            BookId = book!.BookId,
            MemberId = member!.MemberId
        };
        
        //Act
        var responseLoan = await _client.PostAsJsonAsync("/api/loan", requestLoan);
        
        //Assert
        Assert.Equal(HttpStatusCode.Created, responseLoan.StatusCode);
        
        var createdLoan = await responseLoan.Content.ReadFromJsonAsync<LoanDto>();
        Assert.NotNull(createdLoan);
        Assert.Equal(book.BookId, createdLoan.BookId);
        Assert.Equal(member.MemberId, createdLoan.MemberId);
    }
    
    [Fact]
    public async Task CreateLoan_ShouldReturnBadRequest_WhenBookDoesNotExist()
    {
        //Arrange
        var requestMember = new CreateMemberDto
        {
            Name = "Martin Martin",
            Email = "martin@example.com"
        };
        
        var responseMember = await _client.PostAsJsonAsync("/api/member", requestMember);
        var member = await responseMember.Content.ReadFromJsonAsync<MemberDto>();

        var requestLoan = new CreateLoanDto
        {
            BookId = 9999,
            MemberId = member!.MemberId
        };
        
        //Act
        var createdLoan = await _client.PostAsJsonAsync("/api/loan", requestLoan);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, createdLoan.StatusCode);
    }
    
    [Fact]
    public async Task CreateLoan_ShouldReturnBadRequest_WhenMemberDoesNotExist()
    {
        //Arrange
        var requestBook = new CreateBookDto
        {
            Title = "Hardware",
            Author = "Robert Martin",
            PublishedYear = 2008
        };
        
        var responseBook = await _client.PostAsJsonAsync("/api/book", requestBook);
        var book = await responseBook.Content.ReadFromJsonAsync<BookDto>();

        var requestLoan = new CreateLoanDto
        {
            BookId = book!.BookId,
            MemberId = 9999
        };
        
        //Act
        var createdLoan = await _client.PostAsJsonAsync("/api/loan", requestLoan);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, createdLoan.StatusCode);
    }
    
    [Fact]
    public async Task CreateLoan_ShouldReturnBadRequest_WhenBookIsNotAvailable()
    {
        //Arrange
        var requestBook = new CreateBookDto
        {
            Title = "Diagramm",
            Author = "Robert Martin",
            PublishedYear = 2008
        };
        
        var responseBook = await _client.PostAsJsonAsync("/api/book", requestBook);
        var book = await responseBook.Content.ReadFromJsonAsync<BookDto>();

        var requestMember = new CreateMemberDto
        {
            Name = "Sara Blumen",
            Email = "sara@example.com"
        };
        
        var responseMember = await _client.PostAsJsonAsync("/api/member", requestMember);
        var member = await responseMember.Content.ReadFromJsonAsync<MemberDto>();

        var requestLoan = new CreateLoanDto
        {
            BookId = book!.BookId,
            MemberId = member!.MemberId
        };
        
        //Act
        var responseLoan1 = await _client.PostAsJsonAsync("/api/loan", requestLoan);
        var responseLoan2 = await _client.PostAsJsonAsync("/api/loan", requestLoan);
        
        //Assert
        Assert.Equal(HttpStatusCode.Created, responseLoan1.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, responseLoan2.StatusCode);
    }
    
    [Fact]
    public async Task ReturnLoan_ShouldReturnSuccess()
    {
        //Arrange
        var requestBook = new CreateBookDto
        {
            Title = "Daten Bank",
            Author = "Robert Martin",
            PublishedYear = 2008
        };
        
        var responseBook = await _client.PostAsJsonAsync("/api/book", requestBook);
        var book = await responseBook.Content.ReadFromJsonAsync<BookDto>();

        var requestMember = new CreateMemberDto
        {
            Name = "Lara Martin",
            Email = "lara@example.com"
        };
        
        var responseMember = await _client.PostAsJsonAsync("/api/member", requestMember);
        var member = await responseMember.Content.ReadFromJsonAsync<MemberDto>();

        var requestLoan = new CreateLoanDto
        {
            BookId = book!.BookId,
            MemberId = member!.MemberId
        };
        
        var responseLoan = await _client.PostAsJsonAsync("/api/loan", requestLoan);
        Assert.Equal(HttpStatusCode.Created, responseLoan.StatusCode);  
        
        var loan = await responseLoan.Content.ReadFromJsonAsync<LoanDto>();
        
        //Act
        var response = await _client.DeleteAsync($"/api/loan/{loan!.LoanId}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode); 
    }
    
    [Fact]
    public async Task ReturnLoan_ShouldReturnNotFound()
    {
        //Act
        var response = await _client.DeleteAsync("/api/loan/9999");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task ReturnLoan_ShouldReturnBadRequest_WhenAlreadyReturned()
    {
        // Arrange
        var bookRequest = new CreateBookDto
        {
            Title = "Testing",
            Author = "Robert Martin",
            PublishedYear = 2008
        };

        var bookResponse = await _client.PostAsJsonAsync("/api/book", bookRequest);
        var book = await bookResponse.Content.ReadFromJsonAsync<BookDto>();

        var memberRequest = new CreateMemberDto
        {
            Name = "Max Martin",
            Email = "max@example.com"
        };

        var memberResponse = await _client.PostAsJsonAsync("/api/member", memberRequest);
        var member = await memberResponse.Content.ReadFromJsonAsync<MemberDto>();

        var loanRequest = new CreateLoanDto
        {
            BookId = book!.BookId,
            MemberId = member!.MemberId
        };

        var loanResponse = await _client.PostAsJsonAsync("/api/loan", loanRequest);
        Assert.Equal(HttpStatusCode.Created, loanResponse.StatusCode);

        var loan = await loanResponse.Content.ReadFromJsonAsync<LoanDto>();

        // Act
        var firstReturn = await _client.DeleteAsync($"/api/loan/{loan!.LoanId}");

        var secondReturn = await _client.DeleteAsync($"/api/loan/{loan.LoanId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, firstReturn.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, secondReturn.StatusCode);
    }
}