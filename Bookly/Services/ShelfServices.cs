using Bookly.Repository;
using Bookly.Models;

namespace Bookly.Services
{
    public class ShelfServices
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ShelfRepository _shelfRepo;
        private int progress;
        public ShelfServices(ShelfRepository shelfRepo, IHttpContextAccessor contextAccessor)
        {
           this._shelfRepo= shelfRepo;
            this._contextAccessor = contextAccessor;

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
            if(GetShelfById(shelfId).Name=="Have Read")
            {
                IncreaseProgress();
            }
            return _shelfRepo.AddBookToShelf(bookId,shelfId,userId);
        }
        
        public bool RemoveBookFromShelf(int userId, int bookId)
        {
            return _shelfRepo.RemoveBookFromShelf(userId, bookId);
        }

        public void RemoveShelf(int id)
        {
            _shelfRepo.RemoveShelf(id);
        }

        public void IncreaseProgress()
        {
            int progress = _contextAccessor.HttpContext?.Session.GetInt32("Progress") ?? 0;
            progress++;
            _contextAccessor.HttpContext?.Session.SetInt32("Progress", progress);
        }

        public int GetProgress()
        {
            return _contextAccessor.HttpContext?.Session.GetInt32("Progress") ?? 0;
        }
    }
}
