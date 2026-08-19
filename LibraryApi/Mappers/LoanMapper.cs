using LibraryApi.DTOs;
using LibraryApi.Models;

namespace LibraryApi.Mapper;

public class LoanMapper
{
    public static LoanDto ToDto(Loan loan)
    {
        return new LoanDto
        {
            LoanId = loan.LoanId,
            BookId = loan.BookId,
            MemberId = loan.MemberId,
            LoanDate = loan.LoanDate,
            ReturnDate = loan.ReturnDate
        };
    }
}