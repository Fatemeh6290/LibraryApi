using LibraryApi.Interfaces;
using LibraryApi.Models;

namespace LibraryApi.Services;

public class LoanService : ILoanService
{
    private readonly IMemberService _memberService;
    private readonly IBookService _bookService;
    private readonly List<Loan> _loans = new();
    private int _loanId = 1;

    public LoanService(IMemberService memberService, IBookService bookService)
    {
        _memberService = memberService;
        _bookService = bookService;
    }
    
    public bool AddLoan(int bookId, int memberId)
    {
        var book = _bookService.GetBookById(bookId);
        var member = _memberService.GetMemberById(memberId);
        if (member is null || book is null || book.IsAvailable == false)
            return false;
        
        _loans.Add(new Loan
        {
            LoanId = _loanId++,
            BookId = bookId,
            MemberId = memberId,
            LoanDate = DateTime.Now,
            ReturnDate = null,
        });
        book.IsAvailable = false;
        return true;
    }

    public bool ReturnBook(int id)
    {
        var loan = GetLoanById(id);
        
        if (loan is null || loan.ReturnDate is not null)
            return false;
        
        var book = _bookService.GetBookById(loan.BookId);
        
        if (book is null)
            return false;
        
        loan.ReturnDate = DateTime.Now;
        book.IsAvailable = true;
        
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