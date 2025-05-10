using Business_logic.DTOs;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IShelfServices
    {
        bool CreateShelf(ShelfDTO shelf);
        void CreateDefaultShelf(string username);
        List<ShelfDTO> GetUserShelves();
        ShelfDTO GetUserWishList();
       ShelfDTO? GetShelfById(int id);
        bool CheckForBook(ShelfDTO shelf, int bookId);
        bool AddBookToShelf(int bookId, int shelfId);
        bool UpdateBookProgress(CurrentBookDTO book, int progress);
        bool RemoveBookFromShelf(int bookId, int shelfId);
        bool RemoveShelf(int id);
        CurrentBookShelfDTO GetCurrentlyReadingShelf();
    }
}
