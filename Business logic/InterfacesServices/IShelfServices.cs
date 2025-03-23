using Bookly.Data.Models;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IShelfServices
    {
        bool CreateShelf(string name, int id);
        List<Shelf> GetUserShelves(int id);
        List<Book> GetBooksFromShelf(int id);
        Shelf? GetShelfById(int id);
        bool AddBookToShelf(int bookId, int shelfId, int userId);
        bool RemoveBookFromShelf(int userId, int bookId, int shelfId);
        void RemoveShelf(int id);
    }
}
