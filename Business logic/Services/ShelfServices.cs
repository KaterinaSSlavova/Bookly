using Bookly.Business_logic.InterfacesServices;
using Models.Entities;
using Bookly.Data.InterfacesRepo;
using Business_logic.DTOs;
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

        public bool CreateShelf(ShelfDTO shelfDTO)
        {
            UserDTO user = GetUser();
            if (!ValidateShelf(shelfDTO)) return false;
            Shelf shelf = _mapper.Map<Shelf>(shelfDTO);
            return _ishelfRepo.CreateShelf(shelf, user.Id);
        }

        public void CreateDefaultShelf(string username)
        {
            UserDTO? user = _userServices.GetUserByUsername(username);
            ShelfDTO shelf = new ShelfDTO("Have Read", new List<BookDTO>());
            _ishelfRepo.CreateShelf(_mapper.Map<Shelf>(shelf), user.Id);
        }

        public List<ShelfDTO> GetUserShelves()
        {
            List<Shelf> shelf = _ishelfRepo.GetUserShelves(GetUser().Id);
            return ConvertListToDTO(shelf);
        }

        public List<BookDTO> GetBooksFromShelf(int id)
        {
            List<Book> books = _ishelfRepo.GetBooksFromShelf(id);
            return _mapper.Map<List<BookDTO>>(books);
        }

        public ShelfDTO? GetShelfById(int id)
        {
            Shelf? shelf = _ishelfRepo.GetShelfById(id);
            return ConvertToDTO(shelf);
        }

        public bool AddBookToShelf(int bookId, int shelfId)
        {
            if(CheckForBook(shelfId, bookId))
            {
                return false;
            }
            UserDTO user = GetUser();
            CheckForPreviousShelf(bookId);
            if (GetShelfById(shelfId)?.Name == "Have Read" && !CheckForBook(shelfId, bookId))
            {
                GoalDTO? goal = _goalService.GetNewestGoal(true);
                IncreaseProgress(goal);
            } 
            return _ishelfRepo.AddBookToShelf(bookId, shelfId, user.Id);
        }

        private void CheckForPreviousShelf(int bookId)
        {
            UserDTO user = GetUser();
            Shelf oldShelf = _ishelfRepo.GetShelfContainingBook(bookId, user.Id);
            if (oldShelf != null && oldShelf.Name == "Have Read")
            {
                GoalDTO? goal = _goalService.GetNewestGoal(false);
                DecreaseProgress(goal);
            }
        }

        public bool RemoveBookFromShelf(int bookId, int shelfId)
        {   
            UserDTO user = GetUser();
            if (GetShelfById(shelfId)?.Name == "Have Read" && CheckForBook(shelfId, bookId))
            {
                GoalDTO? goal = _goalService.GetNewestGoal(false);
                DecreaseProgress(goal);
            }
            return _ishelfRepo.RemoveBookFromShelf(user.Id, bookId);
        }

        private void DecreaseProgress(GoalDTO goal)
        {
            if (goal != null && goal.CurrentProgress > 0)
            {
                goal.CurrentProgress--;
                _goalService.UpdateGoal(goal);
            }
        }

        private void IncreaseProgress(GoalDTO goal)
        {
            if (goal != null)
            {
                 goal.CurrentProgress ++;
                _goalService.UpdateGoal(goal);
            }
        }

        public bool RemoveShelf(int id)
        {
            return _ishelfRepo.RemoveShelf(id);
        }

        public bool CheckForBook(int shelfId, int bookId)
        {
            foreach (BookDTO book in GetBooksFromShelf(shelfId))
            {
                if (book.Id == bookId)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ValidateShelf(ShelfDTO shelf)
        {
            if(shelf.Name == null) return false;
            List<ShelfDTO> shelves = GetUserShelves();
            foreach (ShelfDTO userShelf in shelves)
            {
                if (shelf.Name.Equals(userShelf.Name, StringComparison.OrdinalIgnoreCase)) return false;
            }
            return true;
        }

        private UserDTO GetUser()
        {
            return _userServices.LoadUser();
        }

        private ShelfDTO ConvertToDTO(Shelf shelf)
        {
            return new ShelfDTO(shelf.Id, shelf.Name, GetBooksFromShelf(shelf.Id));
        }

        private List<ShelfDTO> ConvertListToDTO(List<Shelf> shelfs)
        {
            List<ShelfDTO> shelfDTOs = new List<ShelfDTO>();
            foreach(Shelf shelf in shelfs)
            {
                shelfDTOs.Add(new ShelfDTO(shelf.Id, shelf.Name, GetBooksFromShelf(shelf.Id)));
            }
            return shelfDTOs;
        }
    }
}
