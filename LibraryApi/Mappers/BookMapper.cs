using LibraryApi.DTOs;
using LibraryApi.Models;

namespace LibraryApi.Mapper;

public class BookMapper
{
    public static BookDto ToDto(Book book)
    {
        return new BookDto
        {
            BookId = book.BookId,
            Title = book.Title,
            Author = book.Author,
            PublishedYear = book.PublishedYear,
            IsAvailable = book.IsAvailable
        };
    }
}