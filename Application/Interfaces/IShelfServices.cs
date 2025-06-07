using Business_logic.DTOs;

namespace Interfaces
{
    public interface IShelfServices
    {
        void CreateShelf(ShelfDTO shelf);
        void CreateDefaultShelf(string username);
        List<RegularShelfDTO> GetUserShelves();
        RegularShelfDTO GetUserWishList();
       RegularShelfDTO? GetShelfById(int id);
        bool CheckForBook(RegularShelfDTO shelf, int bookId);
        void AddBookToShelf(BookDTO book, RegularShelfDTO shelf);
        void UpdateBookProgress(CurrentBookDTO book, int progress);
        void RemoveBookFromShelf(int bookId, RegularShelfDTO shelf);
        void RemoveShelf(int id);
        CurrentBookShelfDTO GetCurrentlyReadingShelf();
    }
}
