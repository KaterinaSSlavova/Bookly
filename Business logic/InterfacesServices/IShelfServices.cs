using Models.Entities;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IShelfServices
    {
        bool CreateShelf(Shelf shelf);
        void CreateDefaultShelf();
        List<Shelf> GetUserShelves();
        List<Book> GetBooksFromShelf(int id);
        Shelf? GetShelfById(int id);
        bool AddBookToShelf(int bookId, int shelfId);
        bool RemoveBookFromShelf(int bookId, int shelfId);
        bool RemoveShelf(int id);
    }
}
