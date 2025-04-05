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
        private readonly IGoalServices _goalService;
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private int progress;
        public ShelfServices(IShelfRepository ishelfRepo, IMapper mapper, IUserServices userServices, IGoalServices goalService)
        { 
            _ishelfRepo = ishelfRepo;
            _mapper = mapper;
            _userServices = userServices;
            _goalService = goalService;
        }

        public bool CreateShelf(ShelfViewModel shelfModel)
        {
            User user = GetUser();
            Shelf shelf = _mapper.Map<Shelf>(shelfModel);
            return _ishelfRepo.CreateShelf(shelf, user.Id);
        }

        public void CreateDefaultShelf()
        {
            User user = GetUser();
            ShelfViewModel shelf = new ShelfViewModel()
            {
                Name = "Have Read"
            };
            CreateShelf(shelf);
        }

        public List<Shelf> GetUserShelves()
        {
            return _ishelfRepo.GetUserShelves(GetUser().Id);
        }

        public List<ShelfViewModel> GetUserShelfModel()
        {
            List<Shelf> shelves = GetUserShelves();
            return _mapper.Map<List<ShelfViewModel>>(shelves);
        }

        public List<Book> GetBooksFromShelf(int id)
        {
            List<Book> books = _ishelfRepo.GetBooksFromShelf(id);
            return books;
        }

        public List<BookViewModel> GetBooksOnShelfModel(int id)
        {
            List<Book> books = GetBooksFromShelf(id);
            return _mapper.Map<List<BookViewModel>>(books);
        }

        public ShelfViewModel? GetShelfById(int id)
        {
            Shelf shelf = _ishelfRepo.GetShelfById(id);
            ShelfViewModel shelfModel = _mapper.Map<ShelfViewModel>(shelf);
            return shelfModel;
        }

        public bool AddBookToShelf(int bookId, int shelfId)
        {
            User user = GetUser();
            if (GetShelfById(shelfId)?.Name == "Have Read" && !CheckForBook(shelfId, bookId))
            {
                Goal? goal = _goalService.GetNewestGoal(true);
               
                if(goal!=null)
                {
                    progress = ++ goal.CurrentProgress;
                    SetStatus(progress, goal);
                }
            } 
            return _ishelfRepo.AddBookToShelf(bookId, shelfId, user.Id);
        }

        public bool RemoveBookFromShelf(int bookId, int shelfId)
        {   
            User user = GetUser();
            if (GetShelfById(shelfId)?.Name == "Have Read" && CheckForBook(shelfId, bookId))
            {
                Goal? goal = _goalService.GetNewestGoal(true);
                if (goal != null && goal.CurrentProgress>0)
                {
                    progress = -- goal.CurrentProgress;
                    SetStatus(progress, goal);
                }
            }
            return _ishelfRepo.RemoveBookFromShelf(user.Id, bookId);
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

        public void SetStatus(int progress, Goal goal)
        {
            _goalService.UpdateProgress(goal.Id, progress);
            Status newStatus = Status.Not_started;
            if (goal.CurrentProgress > 0 && goal.CurrentProgress < goal.ReadingGoal)
            {
                newStatus = Status.In_progress;
            }
            else if (goal.CurrentProgress == goal.ReadingGoal)
            {
                newStatus = Status.Completed;
            }
            _goalService.UpdateStatus(newStatus, goal.Id);
        }

        private User GetUser()
        {
            return _userServices.LoadUser();
        }
    }
}
