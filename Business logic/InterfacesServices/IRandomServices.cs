using Models.Enums;
using ViewModels.Model;

namespace Business_logic.InterfacesServices
{
    public interface IRandomServices
    {
        List<BookViewModel> GetUnreadBooks();
        BookViewModel RandomResult();
        List<BookViewModel> FilterBooks(Genre genre, Ratings rating);
        DateWithABookViewModel DateWithBook(string filteredJson);
    }
}
