using Models.Entities;
using Models.Enums;

namespace Bookly.Data.InterfacesRepo
{
    public interface IShelfRepository
    {
        bool CreateShelf(Shelf shelf, int id);
        List<Shelf> GetUserShelves(int id);
        List<Book> GetBooksFromShelf(int id);
        Shelf? GetShelfById(int id);
        bool AddBookToShelf(int bookId, int shelfId, int userId);
        bool SetCurrentBookProgress(int userId, CurrentBook book);
        bool SaveCurrentBookProgress(int userId, int bookId, int progress, Status status);
        Shelf GetShelfContainingBook(int bookId, int userId);
        bool RemoveBookFromShelf(int userId, int bookId);
        void RemoveFromCurrentBookShelf(int userId, int bookId);
        bool RemoveShelf(int id);
        List<CurrentBook> GetBooksFromCurrentlyReadingShelf(int userId);

    }
}
