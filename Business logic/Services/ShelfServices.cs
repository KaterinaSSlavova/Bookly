using Interfaces;
using EFDataLayer.DBContext;
using Business_logic.DTOs;
using AutoMapper;
using Exceptions;

namespace Bookly.Business_logic.Services
{
    public class ShelfServices: IShelfServices
    {
        private readonly IShelfRepository _shelfRepo;
        private readonly IBookServices _bookServices;
        private readonly IGoalServices _goalService;
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private const string completedBooksShelf = "Have Read";
        private const string wishBooksShelf = "Want To Read";
        private const string currentBooksShelf = "Currently Reading";

        public ShelfServices(IShelfRepository shelfRepo, IMapper mapper, IUserServices userServices, IGoalServices goalService, IBookServices bookServices)
        {
            _shelfRepo = shelfRepo;
            _mapper = mapper;
            _userServices = userServices;
            _goalService = goalService;
            _bookServices = bookServices;
        }

        public void CreateShelf(ShelfDTO shelfDTO)
        {
            ValidateShelf(shelfDTO);
            UserDTO user = GetUser();
            Shelf shelf = _mapper.Map<Shelf>(shelfDTO);
            _shelfRepo.CreateShelf(shelf, user.Id);
        }

        public void CreateDefaultShelf(string username)
        {
            UserDTO? user = _userServices.GetUserByUsername(username);
            List<ShelfDTO> defaultShelves = new List<ShelfDTO>()
            {
                new ShelfDTO(completedBooksShelf, user),
                new ShelfDTO(wishBooksShelf, user),
                new ShelfDTO(currentBooksShelf, user)
            };
            foreach (ShelfDTO shelf in defaultShelves)
            {
                _shelfRepo.CreateShelf(_mapper.Map<Shelf>(shelf), user.Id);
            }
        }

        public List<RegularShelfDTO> GetUserShelves()
        {
            UserDTO user = GetUser();
            List<RegularShelf> shelves = _shelfRepo.GetUserRegularShelves(_userServices.ConvertToEntity(user));
            List<RegularShelfDTO> shelfDTOs = _mapper.Map<List<RegularShelfDTO>>(shelves);
            shelfDTOs.ForEach(s => s.Shelf.User = user);
            return shelfDTOs;
        }

        public RegularShelfDTO? GetShelfById(int id)
        {
            RegularShelf? shelf = _shelfRepo.GetShelfById(id);
            return _mapper.Map<RegularShelfDTO>(shelf);
        }

        public RegularShelfDTO GetUserWishList()
        {
            return GetUserShelves().Where(s => s.Shelf.Name == wishBooksShelf).Single();
        }

        public CurrentBookShelfDTO GetCurrentlyReadingShelf()
        {
            User user = _userServices.ConvertToEntity(GetUser());
            CurrentBookShelf shelf = _shelfRepo.GetUserCurrentShelf(user, currentBooksShelf);
            return _mapper.Map<CurrentBookShelfDTO>(shelf);
        }

        public void AddBookToShelf(int bookId, int shelfId)
        {
            RegularShelfDTO shelf = GetShelfById(shelfId);
            BookDTO bookDTO = _bookServices.GetBookById(bookId);   
            if (CheckForBook(shelf, bookId)) throw new BookIsAlreadyOnShelfException(shelf.Shelf.Name, bookDTO.Title);
            CheckForPreviousShelf(bookId);
            if (shelf?.Shelf.Name == completedBooksShelf)
            {
                GoalDTO? goal = _goalService.GetNewestGoal(true);
                IncreaseProgress(goal);
            }
            if (shelf?.Shelf.Name == currentBooksShelf)
            { 
                CurrentBookDTO currentBook = new CurrentBookDTO(bookDTO);
                currentBook.User = GetUser();
                CurrentBook book = _mapper.Map<CurrentBook>(currentBook);
                _shelfRepo.SetCurrentBookProgress(book);
            }
            _shelfRepo.AddBookToShelf(bookId, shelf.Shelf.Id, shelf.Shelf.UserId);
        }

        public void UpdateBookProgress(CurrentBookDTO bookDTO, int progress)
        {
            bookDTO.User = GetUser();
            if (bookDTO.Book.Pages < progress || progress < 0) throw new InvalidProgressException(progress, bookDTO.Book.Pages);
            bookDTO.CurrentProgress = progress;
            bookDTO.Status = Status.Not_started;
            if (progress > 0) bookDTO.Status = Status.In_progress;
            if (progress == bookDTO.Book.Pages) bookDTO.Status = Status.Completed;
            CurrentBook book = _mapper.Map<CurrentBook>(bookDTO);
            _shelfRepo.SaveCurrentBookProgress(book);
        }

        private void CheckForPreviousShelf(int bookId)
        {
            RegularShelfDTO? oldShelf = GetUserShelves().FirstOrDefault(s => s.BooksOnShelf.Any(b => b.Id == bookId));
            if(oldShelf != null)
            {
                RemoveBookFromShelf(bookId, oldShelf.Shelf.Id);
            }
        }

        public void RemoveBookFromShelf(int bookId, int shelfId)
        {   
            RegularShelfDTO shelf = GetShelfById(shelfId);
            if (shelf == null) throw new NullReferenceException("Shelf was not found!");
            bool isBookOnShelf = CheckForBook(shelf, bookId);
            if (isBookOnShelf)
            {
                if (shelf?.Shelf.Name == completedBooksShelf)
                {
                    GoalDTO? goal = _goalService.GetNewestGoal(false);
                    DecreaseProgress(goal);
                }
                if (shelf?.Shelf.Name == currentBooksShelf)
                {
                    _shelfRepo.RemoveFromCurrentBookShelf(shelf.Shelf.UserId, bookId);
                }
                _shelfRepo.RemoveBookFromShelf(shelf.Shelf.UserId,bookId);
            }
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

        public void RemoveShelf(int id)
        {
            if (id <= 0) throw new NullReferenceException("Shelf was not found!");
            _shelfRepo.RemoveShelf(id);
        }
        
        public bool CheckForBook(RegularShelfDTO shelf, int bookId)
        {
            foreach (BookDTO book in shelf.BooksOnShelf)
            {
                if (book.Id == bookId)
                {
                    return true;
                }
            }
            return false;
        }

        public void ValidateShelf(ShelfDTO shelf)
        {
            if (shelf == null || shelf.Name == null) throw new NullReferenceException("Invalid shelf!");
            List<RegularShelfDTO> shelvesDTO = GetUserShelves();
            foreach (RegularShelfDTO userShelf in shelvesDTO)
            {
                if (shelf.Name.Equals(userShelf.Shelf.Name, StringComparison.OrdinalIgnoreCase)) throw new ShelfAlreadyExistsException(shelf.Name);
            }
        }

        private UserDTO GetUser()
        {
            return _userServices.LoadUser();
        }
    }
}
