using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs;

public class CreateLoanDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int MemberId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int BookId { get; set; }
}