using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs;

public class CreateLoanDto
{
    [Range(1, int.MaxValue)]
    public int MemberId { get; set; }
    [Range(1, int.MaxValue)]
    public int BookId { get; set; }
}