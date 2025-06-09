using Business_logic.DTOs;

namespace Interfaces
{
    public interface IBookServices
    {
        void AddBook(BookDTO book);
        List<BookDTO> LoadBooks();
        BookDTO GetBookById(int bookId);
        void UpdateBook(BookDTO book);
        void RemoveBook(int id);
        BookDTO? GetBookFromPlanner(string title, string author, int pages);
    }
}
