using LibraryApi.DTOs;
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
    public ActionResult<List<LoanDto>> GetLoans()
    {
        var loans = _loanService.GetLoans();

        var result = loans.Select(loan => new LoanDto()
        {
            LoanId = loan.LoanId,
            BookId = loan.BookId,
            MemberId = loan.MemberId,
            LoanDate = loan.LoanDate,
            ReturnDate = loan.ReturnDate
        }).ToList();

        return result;
    }

    [HttpGet("{id}")]
    public ActionResult<LoanDto> GetLoanById(int id)
    {
        var loan = _loanService.GetLoanById(id);
        
        if (loan == null)
            return NotFound();
        
        var result = new LoanDto
        {
            LoanId = loan.LoanId,
            BookId = loan.BookId,
            MemberId = loan.MemberId,
            LoanDate = loan.LoanDate,
            ReturnDate = loan.ReturnDate
        };
        
        return result;
    }

    [HttpDelete("{id}")]
    public IActionResult ReturnBook(int id)
    {
        var result = _loanService.ReturnBook(id);
        
        if (!result)
            return NotFound();
        
        return NoContent();
    }

    [HttpPost]
    public IActionResult AddLoan(CreateLoanDto dto)
    {
        var result = _loanService.AddLoan(
            dto.BookId,
            dto.MemberId);
        
        if (!result)
            return BadRequest("Member or Book does not exist, or the book is not available.");

        return Ok();
    }
}