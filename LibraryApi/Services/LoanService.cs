using LibraryApi.Data;
using LibraryApi.Interfaces;
using LibraryApi.Models;

namespace LibraryApi.Services;

public class LoanService : ILoanService
{
    private readonly IMemberService _memberService;
    private readonly IBookService _bookService;
    private readonly LibraryDbContext _context;
    private readonly ILogger<LoanService> _logger;

    public LoanService(LibraryDbContext context, IMemberService memberService, IBookService bookService, ILogger<LoanService> logger)
    {
        _context = context;
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
            BookId = bookId,
            MemberId = memberId,
            LoanDate = DateTime.Now,
            ReturnDate = null,
        };
        book.IsAvailable = false;

        _context.Loans.Add(loan);
        _context.SaveChanges();
        
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
        _context.SaveChanges();
        _logger.LogInformation("Book for loan with id {id} was returned successfully.", id);
        
        return true;
    }

    public List<Loan> GetLoans()
    {
        return _context.Loans.ToList();
    }

    public Loan? GetLoanById(int id)
    {
        return _context.Loans.FirstOrDefault(l => l.LoanId == id);
    }
}