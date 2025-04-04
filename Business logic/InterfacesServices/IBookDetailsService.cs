using Business_logic.DTOs;
using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IBookDetailsService
    {
        BookDetailsDTO CreateDTO(int bookId);
        BookDetailsViewModel GetBookDetails(int bookId);
    }
}
