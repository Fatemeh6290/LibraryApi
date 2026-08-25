using LibraryApi.Data;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace LibraryApiTests;

public class LoanServiceTests
{
    private readonly LibraryDbContext _context;
    private readonly BookService _bookService;
    private readonly MemberService _memberService;
    private readonly LoanService _loanService;

    public LoanServiceTests()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _context = new LibraryDbContext(options);
        _bookService = new BookService(_context, NullLogger<BookService>.Instance);
        _memberService = new MemberService(_context, NullLogger<MemberService>.Instance);
        _loanService = new LoanService(_context, _memberService, _bookService, NullLogger<LoanService>.Instance);
    }
    
    [Fact]
    public void AddLoan_ShouldAddLoan()
    {
        //Arrange
        _bookService.AddBook("UML", "Peter", 2000);
        _memberService.AddMember("Tim", "tim@gmail.com");
        
        //Act
        var loan = _loanService.AddLoan(1, 1);
        var loans = _loanService.GetLoans();

        //Assert
        Assert.Single(loans);
        Assert.Equal(1, loans[0].MemberId);
        Assert.Equal(1, loans[0].BookId);
        Assert.Equal(1, loans[0].LoanId); 
        Assert.NotNull(loan);
    }
    
    [Fact]
    public void AddLoan_ShouldReturnNull_WhenBookDoesNotExist()
    {
        //Arrange
        _memberService.AddMember("Tim", "tim@gmail.com");
        
        //Act
        var result = _loanService.AddLoan(1, 1);
        
        //Assert
        Assert.Null(result);
        Assert.Empty(_loanService.GetLoans());
    }
    
    [Fact]
    public void AddLoan_ShouldReturnNull_WhenMemberDoesNotExist()
    {
        //Arrange
        _bookService.AddBook("UML", "Peter", 2000);

        //Act
        var result = _loanService.AddLoan(1, 1);
        
        //Assert
        Assert.Null(result);
        Assert.Empty(_loanService.GetLoans());
    }
    [Fact]
    
    public void AddLoan_ShouldReturnNull_WhenBookIsNotAvailable()
    {
        //Arrange
        _bookService.AddBook("UML", "Peter", 2000);
        _memberService.AddMember("Tim", "tim@gmail.com");
        
        //Act
        _loanService.AddLoan(1, 1);
        var book = _bookService.GetBookById(1);
        var result = _loanService.AddLoan(1, 1);
        
        //Assert
        Assert.Null(result);
        Assert.NotNull(book);
        Assert.False(book.IsAvailable);
    }
    
    [Fact]
    public void AddLoan_ShouldMakeBookUnavailable()
    {
        //Arrange
        _bookService.AddBook("UML", "Peter", 2000);
        _memberService.AddMember("Tim", "tim@gmail.com");
        
        //Act
        _loanService.AddLoan(1, 1);
        var result = _bookService.GetBookById(1);
        
        //Assert
        Assert.NotNull(result);
        Assert.False(result.IsAvailable);
    }
    
    [Fact]
    public void ReturnBook_ShouldReturnBook()
    {
        //Arrange
        _bookService.AddBook("UML", "Peter", 2000);
        _memberService.AddMember("Tim", "tim@gmail.com");

        //Act
        _loanService.AddLoan(1, 1);
        var result = _loanService.ReturnBook(1);
        var loan = _loanService.GetLoanById(1);
        
        Assert.NotNull(loan);
        var book = _bookService.GetBookById(loan.BookId);
        
        //Assert
        Assert.True(result);
        Assert.NotNull(book);
        Assert.NotNull(loan);
        Assert.NotNull(loan.ReturnDate);
        Assert.True(book.IsAvailable);
    }
    
    [Fact]
    public void ReturnBook_ShouldReturnFalse_WhenLoanDoesNotExist()
    {
        //Act
        var result = _loanService.ReturnBook(2);
        
        //Assert
        Assert.False(result);
    }
    
    [Fact]
    public void ReturnBook_ShouldReturnFalse_WhenLoanAlreadyReturned()
    {
        //Arrange
        _bookService.AddBook("UML", "Peter", 2000);
        _memberService.AddMember("Tim", "tim@gmail.com");

        //Act
        _loanService.AddLoan(1, 1);
        _loanService.ReturnBook(1);
        var result = _loanService.ReturnBook(1);
        
        //Assert
        Assert.False(result);
    }
    
    [Fact]
    public void GetLoanById_ShouldReturnLoan()
    {
        //Arange
        _bookService.AddBook("UML", "Peter", 2000);
        _memberService.AddMember("Tim", "tim@gmail.com");
        
        //Act
        _loanService.AddLoan(1, 1);
        var result = _loanService.GetLoanById(1);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.LoanId);
        Assert.Equal(1, result.BookId);
        Assert.Equal(1, result.MemberId);
    }
    
    [Fact]
    public void GetLoanById_ShouldReturnNull_WhenLoanDoesNotExist()
    {
        //Arange
        _bookService.AddBook("UML", "Peter", 2000);
        _memberService.AddMember("Tim", "tim@gmail.com");
        
        //Act
        _loanService.AddLoan(1, 1);
        var result = _loanService.GetLoanById(2);
        
        //Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void GetLoans_ShouldReturnAllLoans()
    {
        //Arange
        _bookService.AddBook("UML", "Peter", 2000);
        _memberService.AddMember("Tim", "tim@gmail.com");
        
        //Act
        _loanService.AddLoan(1, 1);
        _loanService.ReturnBook(1);
        _loanService.AddLoan(1, 1);
        var result = _loanService.GetLoans();
        
        //Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result.First().LoanId);
        Assert.Equal(2, result.Last().LoanId);
    }
    
    [Fact]
    public void GetLoans_ShouldReturnEmpty_WhenNoLoansExist()
    {
        // Act
        var result = _loanService.GetLoans();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void AddLoan_ShouldHaveNotReturnDate()
    {
        //Arrange
        _bookService.AddBook("UML", "Peter", 2000);
        _memberService.AddMember("Tim", "tim@gmail.com");
        
        //Act
        var loan = _loanService.AddLoan(1, 1);
        
        //Assert
        Assert.NotNull(loan);
        Assert.Null(loan.ReturnDate);
    }
}