using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IShelfServices
    {
        bool CreateShelf(ShelfViewModel shelfModel);
        void CreateDefaultShelf();
        List<ShelfViewModel> GetUserShelves(int id);
        List<BookViewModel> GetBooksFromShelf(int id);
        ShelfViewModel? GetShelfById(int id);
        bool AddBookToShelf(int bookId, int shelfId);
        bool RemoveBookFromShelf(int bookId, int shelfId);
        void RemoveShelf(int id);
    }
}
