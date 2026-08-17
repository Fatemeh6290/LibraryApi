namespace LibraryApi.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int PublishedYear { get; set; }
    public bool IsAvailable { get; set; }
}