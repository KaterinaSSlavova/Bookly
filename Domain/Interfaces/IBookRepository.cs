using Models.Entities;

namespace Interfaces
{
    public interface IBookRepository
    {
        void AddBook(Book book);
        List<Book> LoadBooks();
        Book? GetBookById(int id);
        void UpdateBook(Book book);
        void RemoveBook(int id);
    }
}
