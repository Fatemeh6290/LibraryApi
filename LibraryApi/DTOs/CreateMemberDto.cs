using System.ComponentModel.DataAnnotations;
using LibraryApi.Interfaces;

namespace LibraryApi.DTOs;

public class CreateMemberDto
{
    [Required]
    public string? Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}