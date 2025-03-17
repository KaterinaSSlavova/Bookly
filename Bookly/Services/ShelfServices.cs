using Bookly.Models;
using Bookly.Repository;

namespace Bookly.Services
{
    public class ShelfServices
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ShelfRepository _shelfRepo;
        private readonly GoalRepository _goalRepo;
        private int progress;
        public ShelfServices(ShelfRepository shelfRepo, IHttpContextAccessor contextAccessor, GoalRepository goalRepo)
        {
            this._shelfRepo = shelfRepo;
            this._contextAccessor = contextAccessor;
            this._goalRepo = goalRepo;
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
            if (GetShelfById(shelfId)?.Name == "Have Read")
            {
                UpdateProgress(1);
            }
            return _shelfRepo.AddBookToShelf(bookId, shelfId, userId);
        }

        public bool RemoveBookFromShelf(int userId, int bookId, int shelfId)
        {
            if (GetShelfById(shelfId)?.Name == "Have Read")
            {
                UpdateProgress(-1);
            }
            return _shelfRepo.RemoveBookFromShelf(userId, bookId);
        }

        public void RemoveShelf(int id)
        {
            _shelfRepo.RemoveShelf(id);
        }

        public void UpdateProgress(int updateAmount)
        {
            progress = GetProgress() + updateAmount;
            SaveProgress();
        }

        public int GetProgress()
        {
            return _contextAccessor.HttpContext?.Session.GetInt32("Progress") ?? 0;
        }

        public void SaveProgress()
        {
            _contextAccessor.HttpContext?.Session.SetInt32("Progress", progress);
        }
    }
}
