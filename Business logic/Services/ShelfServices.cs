using Bookly.Business_logic.InterfacesServices;
using Models.Entities;
using Models.Enums;
using Bookly.Data.InterfacesRepo;
using ViewModels.Model;
using AutoMapper;

namespace Bookly.Business_logic.Services
{
    public class ShelfServices: IShelfServices
    {
        private readonly IShelfRepository _ishelfRepo;
        private readonly IGoalServices _igoalService;
        private readonly IMapper _mapper;
        private int progress;
        public ShelfServices(IShelfRepository ishelfRepo, IGoalServices igoalSrevice, IMapper mapper) 
        {
            this._ishelfRepo = ishelfRepo;
            _igoalService = igoalSrevice;
            _mapper = mapper;
        }

        public bool CreateShelf(ShelfViewModel shelfModel, int id)
        {
            Shelf shelf = _mapper.Map<Shelf>(shelfModel);
            return _ishelfRepo.CreateShelf(shelf, id);
        }

        public List<ShelfViewModel> GetUserShelves(int id)
        {
            List<ShelfViewModel> shelves = new List<ShelfViewModel>();
            foreach (Shelf shelf in _ishelfRepo.GetUserShelves(id))
            {
                shelves.Add(_mapper.Map<ShelfViewModel>(shelf));
            }
            return shelves;
        }

        public List<BookViewModel> GetBooksFromShelf(int id)
        {
            List<BookViewModel> bookModels = new List<BookViewModel>();
            foreach(Book book in _ishelfRepo.GetBooksFromShelf(id))
            {
                bookModels.Add(_mapper.Map<BookViewModel>(book));
            }
            return bookModels;
        }

        public ShelfViewModel? GetShelfById(int id)
        {
            Shelf shelf = _ishelfRepo.GetShelfById(id);
            ShelfViewModel shelfModel = _mapper.Map<ShelfViewModel>(shelf);
            return shelfModel;
        }

        public bool AddBookToShelf(int bookId, int shelfId, int userId)
        {
            if (GetShelfById(shelfId)?.Name == "Have Read" && !CheckForBook(shelfId, bookId))
            {
                Goal? goal = _mapper.Map<Goal>(_igoalService.GetNewestGoal(true));
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
                Goal? goal = _mapper.Map<Goal>(_igoalService.GetNewestGoal(true));
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
            foreach(BookViewModel book in GetBooksFromShelf(shelfId))
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
