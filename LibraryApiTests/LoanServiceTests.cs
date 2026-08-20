using LibraryApi.Data;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace LibraryApiTests;

public class LoanServiceTests
{
    [Fact]
    public void AddLoan_ShouldAddLoan()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new LibraryDbContext(options);
        BookService bookService = new BookService(context, NullLogger<BookService>.Instance);
        MemberService memberService = new MemberService(context, NullLogger<MemberService>.Instance);
        LoanService loanService = new LoanService(context, memberService, bookService, NullLogger<LoanService>.Instance);

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
        Assert.NotNull(result1);
    }
    [Fact]
    public void AddLoan_ShouldReturnNull_WhenBookDoesNotExist()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new LibraryDbContext(options);
        BookService bookService = new BookService(context, NullLogger<BookService>.Instance);
        MemberService memberService = new MemberService(context, NullLogger<MemberService>.Instance);
        LoanService loanService = new LoanService(context, memberService, bookService, NullLogger<LoanService>.Instance);
        
        //Act
        memberService.AddMember("Tim", "tim@gmail.com");
        var result = loanService.AddLoan(1, 1);
        
        //Assert
        Assert.Null(result);
        Assert.Empty(loanService.GetLoans());
    }
    [Fact]
    public void AddLoan_ShouldReturnNull_WhenMemberDoesNotExist()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new LibraryDbContext(options);
        BookService bookService = new BookService(context, NullLogger<BookService>.Instance);
        MemberService memberService = new MemberService(context, NullLogger<MemberService>.Instance);
        LoanService loanService = new LoanService(context, memberService, bookService, NullLogger<LoanService>.Instance);

        //Act
        bookService.AddBook("UML", "Peter", 2000);
        var result = loanService.AddLoan(1, 1);
        
        //Assert
        Assert.Null(result);
        Assert.Empty(loanService.GetLoans());
    }
    [Fact]
    public void AddLoan_ShouldReturnNull_WhenBookIsNotAvailable()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new LibraryDbContext(options);
        BookService bookService = new BookService(context, NullLogger<BookService>.Instance);
        MemberService memberService = new MemberService(context, NullLogger<MemberService>.Instance);
        LoanService loanService = new LoanService(context, memberService, bookService, NullLogger<LoanService>.Instance);
        
        //Act
        bookService.AddBook("UML", "Peter", 2000);
        memberService.AddMember("Tim", "tim@gmail.com");
        loanService.AddLoan(1, 1);
        var book = bookService.GetBookById(1);
        var result = loanService.AddLoan(1, 1);
        
        //Assert
        Assert.Null(result);
        Assert.NotNull(book);
        Assert.False(book.IsAvailable);
    }
    [Fact]
    public void AddLoan_ShouldMakeBookUnavailable()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new LibraryDbContext(options);
        BookService bookService = new BookService(context, NullLogger<BookService>.Instance);
        MemberService memberService = new MemberService(context, NullLogger<MemberService>.Instance);
        LoanService loanService = new LoanService(context, memberService, bookService, NullLogger<LoanService>.Instance);
        
        //Act
        bookService.AddBook("UML", "Peter", 2000);
        memberService.AddMember("Tim", "tim@gmail.com");
        loanService.AddLoan(1, 1);
        var result = bookService.GetBookById(1);
        
        //Assert
        Assert.NotNull(result);
        Assert.False(result.IsAvailable);
    }
    [Fact]
    public void ReturnBook_ShouldReturnBook()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new LibraryDbContext(options);
        BookService bookService = new BookService(context, NullLogger<BookService>.Instance);
        MemberService memberService = new MemberService(context, NullLogger<MemberService>.Instance);
        LoanService loanService = new LoanService(context, memberService, bookService, NullLogger<LoanService>.Instance);

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
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new LibraryDbContext(options);
        BookService bookService = new BookService(context, NullLogger<BookService>.Instance);
        MemberService memberService = new MemberService(context, NullLogger<MemberService>.Instance);
        LoanService loanService = new LoanService(context, memberService, bookService, NullLogger<LoanService>.Instance);

        //Act
        var result = loanService.ReturnBook(2);
        
        //Assert
        Assert.False(result);
    }
    [Fact]
    public void ReturnBook_ShouldReturnFalse_WhenLoanAlreadyReturned()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new LibraryDbContext(options);
        BookService bookService = new BookService(context, NullLogger<BookService>.Instance);
        MemberService memberService = new MemberService(context, NullLogger<MemberService>.Instance);
        LoanService loanService = new LoanService(context, memberService, bookService, NullLogger<LoanService>.Instance);

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
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new LibraryDbContext(options);
        BookService bookService = new BookService(context, NullLogger<BookService>.Instance);
        MemberService memberService = new MemberService(context, NullLogger<MemberService>.Instance);
        LoanService loanService = new LoanService(context, memberService, bookService, NullLogger<LoanService>.Instance);

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
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new LibraryDbContext(options);
        BookService bookService = new BookService(context, NullLogger<BookService>.Instance);
        MemberService memberService = new MemberService(context, NullLogger<MemberService>.Instance);
        LoanService loanService = new LoanService(context, memberService, bookService, NullLogger<LoanService>.Instance);
        
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
        var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using var context = new LibraryDbContext(options);
        BookService bookService = new BookService(context, NullLogger<BookService>.Instance);
        MemberService memberService = new MemberService(context, NullLogger<MemberService>.Instance);
        LoanService loanService = new LoanService(context, memberService, bookService, NullLogger<LoanService>.Instance);

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