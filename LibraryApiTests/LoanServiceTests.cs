using LibraryApi.Models;
using LibraryApi.Services;

namespace LibraryApiTests;

public class LoanServiceTests
{
    [Fact]
    public void AddLoan_ShouldAddLoan()
    {
        //Arrange
        BookService bookService = new BookService();
        MemberService memberService = new MemberService();
        LoanService loanService = new LoanService(memberService, bookService);

        //Act
        bookService.AddBook("UML", "Peter", 2000);
        memberService.AddMember("Tim", "tim@gmail.com");
        var result1 = loanService.AddLoan(1, 1);
        var result = loanService.GetLoans();

        //Assert
        Assert.True(result.Count == 1);
        Assert.Equal(1, result[0].MemberId);
        Assert.Equal(1, result[0].BookId);
        Assert.Equal(1, result[0].LoanId); 
        Assert.True(result1);
    }
    [Fact]
    public void AddLoan_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        //Arrange
        BookService bookService = new BookService();
        MemberService memberService = new MemberService();
        LoanService loanService = new LoanService(memberService, bookService);

        //Act
        memberService.AddMember("Tim", "tim@gmail.com");
        var result = loanService.AddLoan(1, 1);
        
        //Assert
        Assert.False(result);
        Assert.Empty(loanService.GetLoans());
    }
    [Fact]
    public void AddLoan_ShouldReturnFalse_WhenMemberDoesNotExist()
    {
        //Arrange
        BookService bookService = new BookService();
        MemberService memberService = new MemberService();
        LoanService loanService = new LoanService(memberService, bookService);

        //Act
        bookService.AddBook("UML", "Peter", 2000);
        var result = loanService.AddLoan(1, 1);
        
        //Assert
        Assert.False(result);
        Assert.Empty(loanService.GetLoans());
    }
    [Fact]
    public void AddLoan_ShouldReturnFalse_WhenBookIsNotAvailable()
    {
        //Arrange
        BookService bookService = new BookService();
        MemberService memberService = new MemberService();
        LoanService loanService = new LoanService(memberService, bookService);

        //Act
        bookService.AddBook("UML", "Peter", 2000);
        memberService.AddMember("Tim", "tim@gmail.com");
        loanService.AddLoan(1, 1);
        var book = bookService.GetBookById(1);
        var result = loanService.AddLoan(1, 1);
        
        //Assert
        Assert.False(result);
        Assert.NotNull(book);
        Assert.False(book.IsAvailable);
    }
    [Fact]
    public void AddLoan_ShouldMakeBookUnavailable()
    {
        //Arrange
        BookService bookService = new BookService();
        MemberService memberService = new MemberService();
        LoanService loanService = new LoanService(memberService, bookService);

        //Act
        bookService.AddBook("UML", "Peter", 2000);
        memberService.AddMember("Tim", "tim@gmail.com");
        loanService.AddLoan(1, 1);
        var book = bookService.GetBookById(1);
        
        //Assert
        Assert.NotNull(book);
        Assert.False(book.IsAvailable);
    }
    [Fact]
    public void ReturnBook_ShouldReturnBook()
    {
        //Arrange
        BookService bookService = new BookService();
        MemberService memberService = new MemberService();
        LoanService loanService = new LoanService(memberService, bookService);

        //Act
        bookService.AddBook("UML", "Peter", 2000);
        memberService.AddMember("Tim", "tim@gmail.com");
        loanService.AddLoan(1, 1);
        var result = loanService.ReturnBook(1);
        var loan = loanService.GetLoanById(1);
        var book = bookService.GetBookById(loan.BookId);
        
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
        //Arrange
        BookService bookService = new BookService();
        MemberService memberService = new MemberService();
        LoanService loanService = new LoanService(memberService, bookService);

        //Act
        var result = loanService.ReturnBook(2);
        
        //Assert
        Assert.False(result);
    }
    [Fact]
    public void ReturnBook_ShouldReturnFalse_WhenLoanAlreadyReturned()
    {
        //Arrange
        BookService bookService = new BookService();
        MemberService memberService = new MemberService();
        LoanService loanService = new LoanService(memberService, bookService);

        //Act
        bookService.AddBook("UML", "Peter", 2000);
        memberService.AddMember("Tim", "tim@gmail.com");
        loanService.AddLoan(1, 1);
        loanService.ReturnBook(1);
        var result = loanService.ReturnBook(1);
        
        //Assert
        Assert.False(result);
    }
    [Fact]
    public void GetLoanById_ShouldReturnLoan()
    {
        BookService bookService = new BookService();
        MemberService memberService = new MemberService();
        LoanService loanService = new LoanService(memberService, bookService);

        //Act
        bookService.AddBook("UML", "Peter", 2000);
        memberService.AddMember("Tim", "tim@gmail.com");
        loanService.AddLoan(1, 1);
        var result = loanService.GetLoanById(1);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.LoanId);
        Assert.Equal(1, result.BookId);
        Assert.Equal(1, result.MemberId);
    }
    [Fact]
    public void GetLoanById_ShouldReturnNull_WhenLoanDoesNotExist()
    {
        BookService bookService = new BookService();
        MemberService memberService = new MemberService();
        LoanService loanService = new LoanService(memberService, bookService);

        //Act
        bookService.AddBook("UML", "Peter", 2000);
        memberService.AddMember("Tim", "tim@gmail.com");
        loanService.AddLoan(1, 1);
        loanService.ReturnBook(1);
        var result = loanService.GetLoanById(2);
        
        //Assert
        Assert.Null(result);
    }
    [Fact]
    public void GetLoans_ShouldReturnAllLoans()
    {
        BookService bookService = new BookService();
        MemberService memberService = new MemberService();
        LoanService loanService = new LoanService(memberService, bookService);

        //Act
        bookService.AddBook("UML", "Peter", 2000);
        memberService.AddMember("Tim", "tim@gmail.com");
        loanService.AddLoan(1, 1);
        loanService.ReturnBook(1);
        loanService.AddLoan(1, 1);
        var result = loanService.GetLoans();
        
        //Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result.First().LoanId);
        Assert.Equal(2, result.Last().LoanId);
    }
}