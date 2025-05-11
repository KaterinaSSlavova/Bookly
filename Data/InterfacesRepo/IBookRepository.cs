using Models.Entities;

namespace Bookly.Data.InterfacesRepo
{
    public interface IBookRepository
    {
        bool AddBook(Book book);
        List<Book> LoadBooks();
        Book? GetBookById(int id);
        bool UpdateBook(Book book);
        void RemoveBook(int id);
    }
}
