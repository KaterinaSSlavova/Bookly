using Models.Entities;
using Models.Enums;

namespace Business_logic.InterfacesServices
{
    public interface IRandomServices
    {
        List<Book> GetUnreadBooks(int userId);
        Book RandomResult(int userId);
        List<Book> FilterBooks(int userId, Genre genre, Ratings rating);
    }
}
