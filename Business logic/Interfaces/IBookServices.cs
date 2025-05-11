using Business_logic.DTOs;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IBookServices
    {
        bool AddBook(BookDTO book);
        List<BookDTO> LoadBooks();
        BookDTO GetBookById(int bookId);
        bool UpdateBook(BookDTO book);
        void RemoveBook(int id);
    }
}
