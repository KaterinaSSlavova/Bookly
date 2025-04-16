using Models.Enums;
using Business_logic.DTOs;

namespace Business_logic.InterfacesServices
{
    public interface IRandomServices
    {
        List<BookDTO> GetUnreadBooks();
        BookDTO RandomResult();
        List<BookDTO> FilterBooks(Genre genre, Ratings rating);
        DateWithABookDTO CreateDateDTO(string filteredJson);
    }
}
