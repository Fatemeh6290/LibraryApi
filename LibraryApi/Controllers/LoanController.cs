using LibraryApi.Interfaces;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoanController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpGet]
    public ActionResult<List<Loan>> GetLoans()
    {
        return _loanService.GetLoans();
    }

    [HttpGet("{id}")]
    public ActionResult<Loan> GetLoan(int id)
    {
        var loan = _loanService.GetLoanById(id);
        
        if (loan == null)
            return NotFound();
        
        return loan;
    }

    [HttpDelete("{id}")]
    public IActionResult ReturnBook(int id)
    {
        var result = _loanService.ReturnBook(id);
        
        if (!result)
            return NotFound();
        
        return NoContent();
    }
}