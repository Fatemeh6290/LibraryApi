using LibraryApi.Models;

namespace LibraryApi.Interfaces;

public interface ILoanService 
{
    Loan? AddLoan(int bookId, int memberId);
    bool ReturnBook(int id);
    List<Loan> GetLoans();
    Loan? GetLoanById(int id);
}