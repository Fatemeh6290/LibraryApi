using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs;

public class CreateBookDto
{
    [Required] 
    public string? Title { get; set; }
    
    [Required]
    public string? Author { get; set; }
    
    [Required]
    [Range(1990, 2026)]
    public int PublishedYear { get; set; }
}