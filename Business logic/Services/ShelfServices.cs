using Bookly.Business_logic.InterfacesServices;
using Models.Entities;
using Models.Enums;
using Bookly.Data.InterfacesRepo;

namespace Bookly.Business_logic.Services
{
    public class ShelfServices: IShelfServices
    {
        private readonly IShelfRepository _ishelfRepo;
        private readonly IGoalServices _igoalService;
        private int progress;
        public ShelfServices(IShelfRepository ishelfRepo, IGoalServices igoalSrevice) 
        {
            this._ishelfRepo = ishelfRepo;
            _igoalService = igoalSrevice;
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
                Goal? goal = _igoalService.GetNewestGoal(true);
                if(goal!=null)
                {
                    progress = ++ goal.CurrentProgress;
                    SetStatus(progress, goal, userId);
                }
            } 
            return _ishelfRepo.AddBookToShelf(bookId, shelfId, userId);
        }

        public bool RemoveBookFromShelf(int userId, int bookId, int shelfId)
        {     
            if (GetShelfById(shelfId)?.Name == "Have Read" && CheckForBook(shelfId, bookId))
            {
                Goal? goal = _igoalService.GetNewestGoal(false);
                if (goal != null && goal.CurrentProgress>0)
                {
                    progress = -- goal.CurrentProgress;
                    SetStatus(progress, goal, userId);
                }
            }
            return _ishelfRepo.RemoveBookFromShelf(userId, bookId);
        }

        public void RemoveShelf(int id)
        {
            _ishelfRepo.RemoveShelf(id);
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

        public void SetStatus(int progress, Goal goal, int userId)
        {
            _igoalService.UpdateProgress(userId, goal.Id, progress);
            Status newStatus = Status.Not_started;
            if (goal.CurrentProgress > 0 && goal.CurrentProgress < goal.ReadingGoal)
            {
                newStatus = Status.In_progress;
            }
            else if (goal.CurrentProgress == goal.ReadingGoal)
            {
                newStatus = Status.Completed;
            }
            _igoalService.UpdateStatus(newStatus, goal.Id, userId);
        }
    }
}
