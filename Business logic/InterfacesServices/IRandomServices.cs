using Models.Enums;
using Models.Entities;
using ViewModels.Model;

namespace Business_logic.InterfacesServices
{
    public interface IRandomServices
    {
        List<Book> GetUnreadBooks();
        Book RandomResult();
        List<Book> FilterBooks(Genre genre, Ratings rating);
        List<BookViewModel> GetUnreadBooksModel();
    }
}
