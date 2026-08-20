using LibraryApi.DTOs;
using LibraryApi.Interfaces;
using LibraryApi.Mapper;
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

        var result = loans.Select(LoanMapper.ToDto).ToList();

        return result;
    }

    [HttpGet("{id}")]
    public ActionResult<LoanDto> GetLoanById(int id)
    {
        var loan = _loanService.GetLoanById(id);
        
        if (loan == null)
            return NotFound();
        
        var result = LoanMapper.ToDto(loan);
        
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
    public ActionResult<LoanDto> AddLoan(CreateLoanDto dto)
    {
        var loan = _loanService.AddLoan(
            dto.BookId,
            dto.MemberId);
        
        if (loan is null)
            return BadRequest("Member or Book does not exist, or the book is not available.");

        var result = LoanMapper.ToDto(loan);
        
        return CreatedAtAction(nameof(GetLoanById),
            new{id = loan.LoanId},
            result);
    }
}