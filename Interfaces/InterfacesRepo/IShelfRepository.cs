using Models.Entities;

namespace Interfaces
{
    public interface IShelfRepository
    {
        void CreateShelf(RegularShelf shelf, int id);
        List<RegularShelf> GetUserRegularShelves(User user);
        CurrentBookShelf? GetUserCurrentShelf(User user, string shelfName);
        List<Book> GetBooksFromShelf(int id);
        RegularShelf? GetShelfById(int id);
        void AddBookToShelf(int bookId, int shelfId, int userId);
        void SetCurrentBookProgress(CurrentBook book);
        void SaveCurrentBookProgress(CurrentBook book);
        void RemoveBookFromShelf(int userId, int bookId);
        void RemoveFromCurrentBookShelf(int userId, int bookId);
        void RemoveShelf(int id);
        List<CurrentBook> GetBooksFromCurrentlyReadingShelf(User user);
    }
}
