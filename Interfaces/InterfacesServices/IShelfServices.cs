using Business_logic.DTOs;

namespace Interfaces
{
    public interface IShelfServices
    {
        void CreateShelf(ShelfDTO shelf);
        void CreateDefaultShelf(string username);
        List<ShelfDTO> GetUserShelves();
        ShelfDTO GetUserWishList();
       ShelfDTO? GetShelfById(int id);
        bool CheckForBook(ShelfDTO shelf, int bookId);
        void AddBookToShelf(int bookId, int shelfId);
        void UpdateBookProgress(CurrentBookDTO book, int progress);
        void RemoveBookFromShelf(int bookId, int shelfId);
        void RemoveShelf(int id);
        CurrentBookShelfDTO GetCurrentlyReadingShelf();
    }
}
