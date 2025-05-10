using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.Business_logic.Services;
using Bookly.Data.InterfacesRepo;
using Business_logic.DTOs;
using Business_logic.Mappers;
using Models.Entities;
using Models.Enums;
using Moq;

namespace Tests
{
    [TestClass]
    public class ShelfServiceTests
    {
        private readonly Mock<IShelfRepository> _shelfRepo;
        private readonly Mock<IBookServices> _bookServices;
        private readonly Mock<IGoalServices> _goalServices;
        private readonly Mock<IUserServices> _userServices;
        private readonly IMapper _mapper;
        private readonly ShelfServices _shelfServices;

        public ShelfServiceTests()
        {
            _shelfRepo = new Mock<IShelfRepository>();
            _bookServices = new Mock<IBookServices>();
            _goalServices = new Mock<IGoalServices>();
            _userServices = new Mock<IUserServices>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ShelfMapper());
                cfg.AddProfile(new BookMapper());
            });
            _mapper = config.CreateMapper();

            _shelfServices = new ShelfServices(_shelfRepo.Object, _mapper, _userServices.Object, _goalServices.Object, _bookServices.Object);
        }

        [TestMethod]
        public void CreateShelf_ShouldReturnTrue_WhenShelfIsValid()
        {
            //Arrange
            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            ShelfDTO shelfDTO = new ShelfDTO("Shelf");
            Shelf shelf = new Shelf("Shelf");
            List<Shelf> shelves = new List<Shelf>();
            _userServices.Setup(s => s.LoadUser()).Returns(user);
            _shelfRepo.Setup(r => r.GetUserShelves(user.Id)).Returns(shelves);
            _shelfRepo.Setup(r => r.CreateShelf(It.Is<Shelf>(s => s.Name == shelf.Name), user.Id)).Returns(true);

            //Act
            bool isCreated = _shelfServices.CreateShelf(shelfDTO);

            //Assert
            Assert.IsTrue(isCreated);
           // Assert.ThrowsException
        }

        [TestMethod]
        public void CreateShelf_ShouldReturnFalse_WhenShelfNull()
        {
            //Arrange
            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            List<Shelf> shelves = new List<Shelf>();
            ShelfDTO? shelfDTO = null;
            _userServices.Setup(r => r.LoadUser()).Returns(user);
            _shelfRepo.Setup(r => r.GetUserShelves(user.Id)).Returns(shelves);

            //Act
            bool isCreated = _shelfServices.CreateShelf(shelfDTO);

            //Assert
            Assert.IsFalse(isCreated);
        }

        [TestMethod]
        public void CreateShelf_ShouldReturnFalse_WhenShelfWithTheSameNameAlreadyExists()
        {
            //Arrange
            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            ShelfDTO shelfDTO = new ShelfDTO("Shelf");
            List<Shelf> shelves = new List<Shelf>() { new Shelf(1, "Shelf") };
            _userServices.Setup(r => r.LoadUser()).Returns(user);
            _shelfRepo.Setup(r => r.GetUserShelves(user.Id)).Returns(shelves);

            //Act
            bool isCreated = _shelfServices.CreateShelf(shelfDTO);

            //Assert
            Assert.IsFalse(isCreated);
        }

        [TestMethod]
        public void AddBookToShelf_ShouldReturnTrue_WhenBookIsNotOnTheShelf()
        {
            //Arrange
            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            List<Book> books = new List<Book>();
            Shelf shelf = new Shelf(1, "Have Read");
            ShelfDTO shelfDTO = new ShelfDTO(1, "Have Read", new List<BookDTO>());
            BookDTO book = new BookDTO()
            {
                Id = 3,
                Title = "New Title",
                Author = "Author",
                Description = "Updated",
                ISBN = "123",
                Genre = Genre.Mystery,
                Pages = 250,
                Picture = null
            };
            GoalDTO goal = new GoalDTO(1, new DateTime(2025, 1, 1), new DateTime(2025, 5, 6), 3, 0, Status.Not_started, user);
            _userServices.Setup(r => r.LoadUser()).Returns(user);
            _shelfRepo.Setup(r => r.GetBooksFromShelf(shelfDTO.Id)).Returns(books);
            _shelfRepo.Setup(r => r.GetShelfContainingBook(book.Id, user.Id)).Returns((Shelf?)null);
            _shelfRepo.Setup(r => r.GetShelfById(shelfDTO.Id)).Returns(shelf);
            _goalServices.Setup(s => s.GetNewestGoal(true)).Returns(goal);
            _goalServices.Setup(s => s.UpdateGoal(It.IsAny<GoalDTO>())).Verifiable();
            _shelfRepo.Setup(r => r.AddBookToShelf(book.Id, shelfDTO.Id, user.Id)).Returns(true);

            //Act 
            bool isBookAdded = _shelfServices.AddBookToShelf(book.Id, shelfDTO.Id);

            //Assert
            Assert.IsTrue(isBookAdded);
            _goalServices.Verify(s => s.UpdateGoal(It.IsAny<GoalDTO>()), Times.Once);
        }

        [TestMethod]
        public void AddBookToShelf_ShouldReturnFalse_WhenBookIsOnShelf()
        {
            //Arrange
            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            Shelf shelf = new Shelf(1, "Have Read");
            BookDTO book = new BookDTO()
            {
                Id = 3,
                Title = "New Title",
                Author = "Author",
                Description = "Updated",
                ISBN = "123",
                Genre = Genre.Mystery,
                Pages = 250,
                Picture = null
            };
            ShelfDTO shelfDTO = new ShelfDTO(1, "Have Read", new List<BookDTO>() { book });
            List<Book> books = new List<Book>() { new Book(3, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250) };
            _userServices.Setup(r => r.LoadUser()).Returns(user);
            _shelfRepo.Setup(r => r.GetBooksFromShelf(shelfDTO.Id)).Returns(books);

            //Act 
            bool isBookAdded = _shelfServices.AddBookToShelf(book.Id, shelfDTO.Id);

            //Assert
            Assert.IsFalse(isBookAdded);
        }

        [TestMethod]
        public void AddBookToShelf_ShouldReturnTrue_WhenBookIsMovedToCurrentlyReadingShelfFromHaveReadShelf()
        {
            //Arrange
            int userId = 1;
            int bookId = 2;
            int currentShelfId = 3;
            int completedShelfId = 4;
            UserDTO user = new UserDTO(userId, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            Book book = new Book(bookId, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250);
            BookDTO bookDTO = new BookDTO { Id = bookId, Title = "New Title", Author = "Author", Description = "Updated", ISBN = "123", Genre = Genre.Mystery, Pages = 250, Picture = null };
            GoalDTO goal = new GoalDTO(1, new DateTime(2025, 1, 1), new DateTime(2025, 5, 6), 3, 2, Status.Not_started, user);

            _userServices.Setup(s => s.LoadUser()).Returns(user);
            _shelfRepo.Setup(r => r.GetBooksFromShelf(currentShelfId)).Returns(new List<Book>());
            _shelfRepo.Setup(r => r.GetShelfContainingBook(bookId, userId)).Returns(new Shelf(completedShelfId, "Have Read"));
            _goalServices.Setup(s => s.GetNewestGoal(false)).Returns(goal);
            _goalServices.Setup(s => s.UpdateGoal(It.IsAny<GoalDTO>())).Verifiable();
            _shelfRepo.Setup(r => r.GetShelfById(currentShelfId)).Returns(new Shelf(currentShelfId, "Currently Reading"));
            _bookServices.Setup(s => s.GetBookById(bookId)).Returns(bookDTO);
            _shelfRepo.Setup(s => s.SetCurrentBookProgress(userId, It.IsAny<CurrentBook>())).Verifiable();
            _shelfRepo.Setup(r => r.AddBookToShelf(bookId, currentShelfId, userId)).Returns(true);

            //Act
            bool isBookAdded = _shelfServices.AddBookToShelf(bookId, currentShelfId);

            //Assert
            Assert.IsTrue(isBookAdded);
            _goalServices.Verify(r => r.UpdateGoal(It.IsAny<GoalDTO>()), Times.Once);
            _shelfRepo.Verify(r => r.SetCurrentBookProgress(userId, It.IsAny<CurrentBook>()), Times.Once);
        }

        [TestMethod]
        public void UpdateBookProgress_ShouldReturnTrue_WhenProgressIsBetweenZeroAndMaxPages()
        {
            //Arrange
            int bookId = 1;
            int progress = 49;
            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            CurrentBookDTO bookDTO = new CurrentBookDTO(new BookDTO { Id = bookId, Title = "New Title", Author = "Author", Description = "Updated", ISBN = "123", Genre = Genre.Mystery, Pages = 250, Picture = null }, 0, Status.Not_started);
            _userServices.Setup(s => s.LoadUser()).Returns(user);
            _shelfRepo.Setup(r => r.SaveCurrentBookProgress(user.Id, bookId, progress, It.IsAny<Status>())).Returns(true);

            //Act
            bool isProgressUpdated = _shelfServices.UpdateBookProgress(bookDTO, progress);

            //Assert
            Assert.IsTrue(isProgressUpdated);
        }

        [TestMethod]
        public void UpdateBookProgress_ShouldReturnFalse_WhenProgressIsLowerThanZero()
        {
            //Arrange
            int bookId = 1;
            int progress = -49;
            CurrentBookDTO bookDTO = new CurrentBookDTO(new BookDTO { Id = bookId, Title = "New Title", Author = "Author", Description = "Updated", ISBN = "123", Genre = Genre.Mystery, Pages = 250, Picture = null }, 0, Status.Not_started);

            //Act
            bool isProgressUpdated = _shelfServices.UpdateBookProgress(bookDTO, progress);

            //Assert
            Assert.IsFalse(isProgressUpdated);
        }

        [TestMethod]
        public void UpdateBookProgress_ShouldReturnFalse_WhenProgressIsBiggerThanMAxPages()
        {
            //Arrange
            int bookId = 1;
            int progress = 300;
            CurrentBookDTO bookDTO = new CurrentBookDTO(new BookDTO { Id = bookId, Title = "New Title", Author = "Author", Description = "Updated", ISBN = "123", Genre = Genre.Mystery, Pages = 250, Picture = null }, 0, Status.Not_started);

            //Act
            bool isProgressUpdated = _shelfServices.UpdateBookProgress(bookDTO, progress);

            //Assert
            Assert.IsFalse(isProgressUpdated);
        }

        [TestMethod]
        public void RemoveBookFromShelf_ShouldReturnTrue_WhenBookIsOnHaveReadShelf()
        {
            //Arrange
            BookDTO book = new BookDTO { Id = 1, Title = "New Title", Author = "Author", Description = "Updated", ISBN = "123", Genre = Genre.Mystery, Pages = 250, Picture = null };
            Shelf completedShelf = new Shelf(1, "Have Read");
            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            GoalDTO goal = new GoalDTO(1, new DateTime(2025, 1, 1), new DateTime(2025, 5, 6), 3, 2, Status.In_progress, user);

            _userServices.Setup(s => s.LoadUser()).Returns(user);
            _shelfRepo.Setup(r => r.GetShelfById(completedShelf.Id)).Returns(completedShelf);
            _shelfRepo.Setup(r => r.GetBooksFromShelf(completedShelf.Id)).Returns(new List<Book>() { new Book(1, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250) });
            _goalServices.Setup(r => r.GetNewestGoal(false)).Returns(goal);
            _goalServices.Setup(r => r.UpdateGoal(It.IsAny<GoalDTO>())).Verifiable();
            _shelfRepo.Setup(r => r.RemoveBookFromShelf(user.Id, book.Id)).Returns(true);

            //Act
            bool isRemoved = _shelfServices.RemoveBookFromShelf(book.Id, completedShelf.Id);

            //Assert
            Assert.IsTrue(isRemoved);
           _goalServices.Verify(r => r.UpdateGoal(goal), Times.Once);
        }

        [TestMethod]
        public void RemoveBookFromShelf_ShouldReturnTrue_WhenBookIsOnCurrentlyReadingShelf()
        {
            //Arrange
            CurrentBook book = new CurrentBook(1, 23, Status.In_progress);
            Shelf currentShelf = new Shelf(1, "Currently Reading");
            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);

            _userServices.Setup(s => s.LoadUser()).Returns(user);
            _shelfRepo.Setup(r => r.GetShelfById(currentShelf.Id)).Returns(currentShelf);
            _shelfRepo.Setup(r => r.GetBooksFromShelf(currentShelf.Id)).Returns(new List<Book>() { new Book(1, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250) });
            _shelfRepo.Setup(r => r.RemoveFromCurrentBookShelf(user.Id, book.Id)).Verifiable();
            _shelfRepo.Setup(r => r.RemoveBookFromShelf(user.Id, book.Id)).Returns(true);

            //Act
            bool isRemoved = _shelfServices.RemoveBookFromShelf(book.Id, currentShelf.Id);

            //Assert
            Assert.IsTrue(isRemoved);
            _shelfRepo.Verify(r => r.RemoveFromCurrentBookShelf(user.Id, book.Id), Times.Once);
        }

        [TestMethod]
        public void RemoveBookFromShelf_ShouldReturnFalse_WhenBookIsNotOnShelf()
        {
            //Arrange
            BookDTO book = new BookDTO { Id = 1, Title = "New Title", Author = "Author", Description = "Updated", ISBN = "123", Genre = Genre.Mystery, Pages = 250, Picture = null };
            Shelf completedShelf = new Shelf(1, "Have Read");
            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            GoalDTO goal = new GoalDTO(1, new DateTime(2025, 1, 1), new DateTime(2025, 5, 6), 3, 2, Status.In_progress, user);

            _userServices.Setup(s => s.LoadUser()).Returns(user);
            _shelfRepo.Setup(r => r.GetShelfById(completedShelf.Id)).Returns(completedShelf);
            _shelfRepo.Setup(r => r.GetBooksFromShelf(completedShelf.Id)).Returns(new List<Book>());

            //Act
            bool isRemoved = _shelfServices.RemoveBookFromShelf(book.Id, completedShelf.Id);

            //Assert
            Assert.IsFalse(isRemoved);
        }
    }
}

