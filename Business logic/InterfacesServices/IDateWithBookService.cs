using Business_logic.DTOs;
using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IDateWithBookService
    {
        DateWithABookDTO CreateDateDTO(string filteredJson);
        DateWithABookViewModel GetDateWithABookModel(string filteredJson);
    }
}
