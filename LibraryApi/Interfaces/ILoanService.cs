using LibraryApi.Models;

namespace LibraryApi.Interfaces;

public interface ILoanService 
{
    bool AddLoan(int bookId, int memberId);
    bool ReturnBook(int id);
    List<Loan> GetLoans();
    Loan? GetLoanById(int id);
}