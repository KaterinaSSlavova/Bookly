using Business_logic.DTOs;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IBookServices
    {
        void AddBook(BookDTO book);
        List<BookDTO> LoadBooks();
        BookDTO GetBookById(int bookId);
        void UpdateBook(BookDTO book);
        void RemoveBook(int id);
    }
}
