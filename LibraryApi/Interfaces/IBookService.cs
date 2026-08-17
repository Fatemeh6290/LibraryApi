using LibraryApi.Models;

namespace LibraryApi.Interfaces;

public interface IBookService
{
        List<Book>? GetBooks();
        Book? GetBookById(int id);
        bool AddBook(string title, string author, int publishedYear);
        bool DeleteBook(int id);
}