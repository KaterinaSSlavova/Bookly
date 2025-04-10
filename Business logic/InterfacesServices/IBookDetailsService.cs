using Business_logic.DTOs;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IBookDetailsService
    {
        public BookDetailsDTO CreateDetailsDTO(int bookId);
    }
}
