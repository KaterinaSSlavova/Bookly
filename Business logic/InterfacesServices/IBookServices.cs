using ViewModels.Model;
using Models.Entities;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IBookServices
    {
        bool AddBook(AddBookModel bookModel);
        List<Book> LoadBooks();
        List<BookViewModel> GetAllBooksViewModel();
        Book? GetBookById(int id);
        void RemoveBook(int id);
    }
}
