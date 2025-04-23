using Business_logic.DTOs;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IShelfServices
    {
        bool CreateShelf(ShelfDTO shelf);
        void CreateDefaultShelf(string username);
        List<ShelfDTO> GetUserShelves();
        List<BookDTO> GetBooksFromShelf(int id);
        ShelfDTO GetUserWishList();
        ShelfDTO? GetShelfById(int id);
        bool CheckForBook(int shelfId, int bookId);
        bool AddBookToShelf(int bookId, int shelfId);
        bool RemoveBookFromShelf(int bookId, int shelfId);
        bool RemoveShelf(int id);
    }
}
