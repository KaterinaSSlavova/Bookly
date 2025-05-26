using EFDataLayer.DBContext;
using Business_logic.DTOs;

namespace Interfaces
{
    public interface IRandomServices
    {
        List<BookDTO> GetUnreadBooks();
        BookDTO RandomResult();
        List<BookDTO> FilterBooks(Genre genre, Ratings rating);
        void AddToWishList(BookDTO book);
        DateWithABookDTO CreateDateDTO(string filteredJson);
    }
}
