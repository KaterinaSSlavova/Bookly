using Bookly.Repository;
using Bookly.Models;

namespace Bookly.Services
{
    public class ShelfServices
    {
        private readonly ShelfRepository _shelfRepo;
        public ShelfServices(ShelfRepository shelfRepo)
        {
           this._shelfRepo= shelfRepo;
        }

        public bool CreateShelf(string name, int id)
        {  
            return _shelfRepo.CreateShelf(name, id); 
        }

        public List<Shelf> GetUserShelves(int id)
        {
            return _shelfRepo.GetUserShelves(id);
        }

        public List<Book> GetBooksFromShelf(int id)
        {
            return _shelfRepo.GetBooksFromShelf(id);
        }

        public Shelf? GetShelfById(int id)
        { 
            return _shelfRepo.GetShelfById(id);
        }

        public bool AddBookToShelf(int bookId, int shelfId, int userId)
        {
            return _shelfRepo.AddBookToShelf(bookId,shelfId,userId);
        }

        public void RemoveShelf(int id)
        {
            _shelfRepo.RemoveShelf(id);
        }
    }
}
