using Models.Entities;
using Business_logic.DTOs;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IShelfServices
    {
        bool CreateShelf(ShelfDTO shelf);
        void CreateDefaultShelf();
        List<ShelfDTO> GetUserShelves();
        List<BookDTO> GetBooksFromShelf(int id);
        ShelfDTO? GetShelfById(int id);
        bool AddBookToShelf(int bookId, int shelfId);
        bool RemoveBookFromShelf(int bookId, int shelfId);
        bool RemoveShelf(int id);
    }
}
