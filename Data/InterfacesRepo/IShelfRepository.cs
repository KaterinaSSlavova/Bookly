using Models.Entities;

namespace Bookly.Data.InterfacesRepo
{
    public interface IShelfRepository
    {
        bool CreateShelf(RegularShelf shelf, int id);
        List<RegularShelf> GetUserRegularShelves(User user);
        CurrentBookShelf? GetUserCurrentShelf(User user, string shelfName);
        List<Book> GetBooksFromShelf(int id);
        RegularShelf? GetShelfById(int id);
        bool AddBookToShelf(int bookId, int shelfId, int userId);
        bool SetCurrentBookProgress(CurrentBook book);
        bool SaveCurrentBookProgress(CurrentBook book);
        bool RemoveBookFromShelf(int userId, int bookId);
        void RemoveFromCurrentBookShelf(int userId, int bookId);
        bool RemoveShelf(int id);
        List<CurrentBook> GetBooksFromCurrentlyReadingShelf(User user);
    }
}
