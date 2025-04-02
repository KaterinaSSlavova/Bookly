using Models.Entities;

namespace Bookly.Data.InterfacesRepo
{
    public interface IShelfRepository
    {
        bool CreateShelf(Shelf shelf, int id);
        List<Shelf> GetUserShelves(int id);
        List<Book> GetBooksFromShelf(int id);
        Shelf? GetShelfById(int id);
        bool AddBookToShelf(int bookId, int shelfId, int userId);
        bool RemoveBookFromShelf(int userId, int bookId);
        void RemoveShelf(int id);

    }
}
