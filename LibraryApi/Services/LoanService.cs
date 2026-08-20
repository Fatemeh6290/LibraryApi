using LibraryApi.Interfaces;
using LibraryApi.Models;

namespace LibraryApi.Services;

public class LoanService : ILoanService
{
    private readonly IMemberService _memberService;
    private readonly IBookService _bookService;
    private readonly List<Loan> _loans = new();
    private readonly ILogger<LoanService> _logger;
    private int _loanId = 1;

    public LoanService(IMemberService memberService, IBookService bookService, ILogger<LoanService> logger)
    {
        _memberService = memberService;
        _bookService = bookService;
        _logger = logger;
    }
    
    public Loan? AddLoan(int bookId, int memberId)
    {
        var book = _bookService.GetBookById(bookId);
        var member = _memberService.GetMemberById(memberId);
        
        if (member is null || book is null || !book.IsAvailable)
        {
            _logger.LogWarning("Loan could not be created. Member with id {MemberId} or book with {BookId} is invalid, or book is unavailable.", memberId, bookId);
            return null;
        }
        
        var loan = new Loan
        {
            LoanId = _loanId++,
            BookId = bookId,
            MemberId = memberId,
            LoanDate = DateTime.Now,
            ReturnDate = null,
        };
        book.IsAvailable = false;

        _loans.Add(loan);
        _logger.LogInformation("The Loan with book id {BookId} and member id {MemberId} is added.", loan.BookId, loan.MemberId);
 
        return loan;
    }

    public bool ReturnBook(int id)
    {
        var loan = GetLoanById(id);

        if (loan is null || loan.ReturnDate is not null)
        {
            _logger.LogWarning("Loan with id {LoanId} was not found or has already been returned.", id);
            return false;
        }
        
        var book = _bookService.GetBookById(loan.BookId);

        if (book is null)
        {
            _logger.LogWarning("Book with id {BookId} is null.", loan.BookId);
            return false;
        }
        
        loan.ReturnDate = DateTime.Now;
        book.IsAvailable = true;
        _logger.LogInformation("Book for loan with id {id} was returned successfully.", id);
        
        return true;
    }

    public List<Loan> GetLoans()
    {
        return _loans.ToList();
    }

    public Loan? GetLoanById(int id)
    {
        return _loans.FirstOrDefault(l => l.LoanId == id);
    }
}