using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs;

public class CreateBookDto
{
    [Required] 
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Author { get; set; } = string.Empty;
    
    [Required]
    [Range(1990, 2026)]
    public int PublishedYear { get; set; }
}