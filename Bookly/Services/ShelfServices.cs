using System.Runtime.CompilerServices;
using Bookly.Interfaces;
using Bookly.Models;
using Bookly.Repository;

namespace Bookly.Services
{
    public class ShelfServices: IShelfServices
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IShelfRepository _ishelfRepo;
        private readonly IGoalRepository _igoalRepo;
        private int progress;
        public ShelfServices(IShelfRepository ishelfRepo, IHttpContextAccessor contextAccessor, IGoalRepository igoalRepo)
        {
            this._ishelfRepo = ishelfRepo;
            this._contextAccessor = contextAccessor;
            this._igoalRepo = igoalRepo;    
        }

        public bool CreateShelf(string name, int id)
        {
            return _ishelfRepo.CreateShelf(name, id);
        }

        public List<Shelf> GetUserShelves(int id)
        {
            return _ishelfRepo.GetUserShelves(id);
        }

        public List<Book> GetBooksFromShelf(int id)
        {
            return _ishelfRepo.GetBooksFromShelf(id);
        }

        public Shelf? GetShelfById(int id)
        {
            return _ishelfRepo.GetShelfById(id);
        }

        public bool AddBookToShelf(int bookId, int shelfId, int userId)
        {
            if (GetShelfById(shelfId)?.Name == "Have Read" && !CheckForBook(shelfId, bookId))
            { 
                UpdateProgress(1);
            }
            return _ishelfRepo.AddBookToShelf(bookId, shelfId, userId);
        }

        public bool RemoveBookFromShelf(int userId, int bookId, int shelfId)
        {     
            if (GetShelfById(shelfId)?.Name == "Have Read" && CheckForBook(shelfId, bookId))
            {
                UpdateProgress(-1);
            }
            return _ishelfRepo.RemoveBookFromShelf(userId, bookId);
        }

        public void RemoveShelf(int id)
        {
            _ishelfRepo.RemoveShelf(id);
        }

        public void UpdateProgress(int updateAmount)
        {
            progress = GetProgress() + updateAmount;
            SaveProgress();
        }

        public int GetProgress()
        {
            return _contextAccessor.HttpContext?.Session.GetInt32("Progress") ?? _igoalRepo.GetNewestGoal().CurrentProgress;
        }

        public void SaveProgress()
        {
            _contextAccessor.HttpContext?.Session.SetInt32("Progress", progress);
        }

        public bool CheckForBook(int shelfId, int bookId)
        {
            foreach(Book book in GetBooksFromShelf(shelfId))
            {
                if(book.Id == bookId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
