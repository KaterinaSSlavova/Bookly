using ViewModels.Model;
using Models.Entities;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IShelfServices
    {
        bool CreateShelf(ShelfViewModel shelfModel);
        void CreateDefaultShelf();
        List<Shelf> GetUserShelves();
        List<ShelfViewModel> GetUserShelfModel();
        List<BookViewModel> GetBooksFromShelf(int id);
        ShelfViewModel? GetShelfById(int id);
        bool AddBookToShelf(int bookId, int shelfId);
        bool RemoveBookFromShelf(int bookId, int shelfId);
        void RemoveShelf(int id);
    }
}
