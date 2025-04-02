using Models.Entities;
using Models.Enums;
using ViewModels.Model;

namespace Business_logic.InterfacesServices
{
    public interface IRandomServices
    {
        List<BookViewModel> GetUnreadBooks(int userId);
        BookViewModel RandomResult(int userId);
        List<BookViewModel> FilterBooks(int userId, Genre genre, Ratings rating);
    }
}
