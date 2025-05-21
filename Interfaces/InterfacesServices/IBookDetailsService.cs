using Business_logic.DTOs;

namespace Interfaces
{
    public interface IBookDetailsService
    {
        BookDetailsDTO CreateDetailsDTO(int bookId);
    }
}
