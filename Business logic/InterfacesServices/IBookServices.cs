using Bookly.Data.Models;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IBookServices
    {
        bool AddBook(Book book);
        List<Book> LoadBooks();
        Book? GetBookById(int id);
        void RemoveBook(int id);
    }
}
