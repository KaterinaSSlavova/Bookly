using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.Business_logic.Services;
using Bookly.Data.InterfacesRepo;
using Business_logic.DTOs;
using Business_logic.Exceptions;
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
                cfg.AddProfile(new UserMapper());
            });
            _mapper = config.CreateMapper();

            _shelfServices = new ShelfServices(_shelfRepo.Object, _mapper, _userServices.Object, _goalServices.Object, _bookServices.Object);
        }

        [TestMethod]
        public void CreateShelf_ShouldExecuteRepoUpdate_WhenShelfIsValid()
        {
            //Arrange
            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
            ShelfDTO shelfDTO = new ShelfDTO("Shelf");
            List<RegularShelf> shelves = new List<RegularShelf>();
            _userServices.Setup(s => s.LoadUser()).Returns(userDTO);
            _userServices.Setup(s => s.ConvertToEntity(userDTO)).Returns(user);
            _shelfRepo.Setup(r => r.GetUserRegularShelves(user)).Returns(shelves);
            _shelfRepo.Setup(r => r.CreateShelf(It.IsAny<RegularShelf>(), user.Id)).Verifiable();

            //Act
            _shelfServices.CreateShelf(shelfDTO);
            
            //Assert
            _shelfRepo.Verify(r => r.CreateShelf(It.IsAny<RegularShelf>(), user.Id));
        }

        [TestMethod]
        public void CreateShelf_ShouldThrowException_WhenShelfNull()
        {
            //Arrange
            User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
            ShelfDTO? shelfDTO = null;
            _shelfRepo.Setup(r => r.GetUserRegularShelves(user)).Returns(It.IsAny<List<RegularShelf>>());

            //Act and Assert
            Assert.ThrowsException<ServiceValidationException>(() => _shelfServices.CreateShelf(shelfDTO));
        }

        [TestMethod]
        public void CreateShelf_ShouldThrowException_WhenShelfWithTheSameNameAlreadyExists()
        {
            //Arrange
            User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            ShelfDTO shelfDTO = new ShelfDTO("Shelf");
            List<RegularShelf> shelves = new List<RegularShelf>() { new RegularShelf(1, "Shelf", new List<Book>()) };

            _userServices.Setup(r => r.ConvertToEntity(It.IsAny<UserDTO>())).Returns(user);
            _userServices.Setup(r => r.LoadUser()).Returns(userDTO);
            _shelfRepo.Setup(r => r.GetUserRegularShelves(user)).Returns(shelves);

            //Act and Assert
            Assert.ThrowsException<ShelfAlreadyExistsException>(() => _shelfServices.CreateShelf(shelfDTO));
        }

        [TestMethod]
        public void RemoveShelf_ShouldExecuteMethodOnce_WhenShelfIdIsNotZero()
        {
            //Arrange
            int shelfId = 1;
            _shelfRepo.Setup(r => r.RemoveShelf(shelfId)).Verifiable();

            //Act
            _shelfServices.RemoveShelf(shelfId);

            //Assert
            _shelfRepo.Verify(r => r.RemoveShelf(shelfId), Times.Once);
        }

        [TestMethod]
        public void RemoveShelf_ShouldThrowException_WhenShelfIdIsNegative()
        {
            //Arrange
            int shelfId = -1;

            //Act and Assert
            Assert.ThrowsException<ServiceValidationException>(() => _shelfServices.RemoveShelf(shelfId));
        }

        //[TestMethod]
        //public void AddBookToShelf_ShouldReturnTrue_WhenBookIsNotOnTheShelf()
        //{
        //    //Arrange
        //    User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
        //    UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
        //    List<Book> books = new List<Book>();
        //    RegularShelf shelf = new RegularShelf(1, "Have Read", user, new List<Book>());
        //    ShelfDTO shelfDTO = new ShelfDTO(1, "Have Read", new List<BookDTO>());
        //    BookDTO book = new BookDTO(3, null, "New Title", "Author", "Description", "123", Genre.Mystery, 250);
        //    GoalDTO goal = new GoalDTO(1, new DateTime(2025, 1, 1), new DateTime(2025, 5, 6), 3, 0, Status.Not_started, userDTO);
        //    _userServices.Setup(r => r.LoadUser()).Returns(userDTO);
        //    _shelfRepo.Setup(r => r.GetShelfById(shelfDTO.Id)).Returns(shelf);
        //    _shelfRepo.Setup(r => r.GetUserRegularShelves(user)).Returns(It.IsAny<List<RegularShelf>>());
        //    _goalServices.Setup(s => s.GetNewestGoal(true)).Returns(goal);
        //    _goalServices.Setup(s => s.UpdateGoal(It.IsAny<GoalDTO>())).Verifiable();
        //    _shelfRepo.Setup(r => r.AddBookToShelf(book.Id, shelfDTO.Id, user.Id)).Returns(true);

        //    //Act 
        //    bool isBookAdded = _shelfServices.AddBookToShelf(book.Id, shelfDTO.Id);

        //    //Assert
        //    Assert.IsTrue(isBookAdded);
        //    _goalServices.Verify(s => s.UpdateGoal(It.IsAny<GoalDTO>()), Times.Once);
        //}

        //[TestMethod]
        //public void AddBookToShelf_ShouldReturnFalse_WhenBookIsOnShelf()
        //{
        //    //Arrange
        //    RegularShelf shelf = new RegularShelf(1, "Have Read", new List<Book>() 
        //    { new Book(3, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250) });
        //    BookDTO book = new BookDTO(3, null, "New Title", "Author", "Description", "123", Genre.Mystery, 250);
        //    _shelfRepo.Setup(r => r.GetShelfById(shelf.Id)).Returns(shelf);

        //    //Act 
        //    bool isBookAdded = _shelfServices.AddBookToShelf(book.Id, shelf.Id);

        //    //Assert
        //    Assert.IsFalse(isBookAdded);
        //}

        //[TestMethod]
        //public void AddBookToShelf_ShouldReturnTrue_WhenBookIsMovedToCurrentlyReadingShelfFromHaveReadShelf()
        //{
        //    //Arrange
        //    User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
        //    UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
        //    Book book = new Book(2, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250);
        //    BookDTO bookDTO = new BookDTO(2, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250);
        //    GoalDTO goal = new GoalDTO(1, new DateTime(2025, 1, 1), new DateTime(2025, 5, 6), 3, 2, Status.Not_started, userDTO);
        //    RegularShelf completedShelf = new RegularShelf(2, "Have Read", user, new List<Book>() { book });
        //    RegularShelf currentShelf = new RegularShelf(1, "Currently Reading", user, new List<Book>());
        //    List<RegularShelf> shelves = new List<RegularShelf>() { currentShelf, completedShelf };

        //    _shelfRepo.Setup(r => r.GetShelfById(currentShelf.Id)).Returns(currentShelf);
        //    _userServices.Setup(s => s.LoadUser()).Returns(userDTO);
        //    _userServices.Setup(s => s.ConvertToEntity(userDTO)).Returns(user);
        //    _shelfRepo.Setup(r => r.GetUserRegularShelves(user)).Returns(shelves);
        //    _shelfRepo.Setup(r => r.GetShelfById(completedShelf.Id)).Returns(completedShelf);
        //    _goalServices.Setup(s => s.GetNewestGoal(false)).Returns(goal);
        //    _goalServices.Setup(s => s.UpdateGoal(It.IsAny<GoalDTO>())).Verifiable();
        //    _shelfRepo.Setup(r => r.RemoveBookFromShelf(user.Id, book.Id)).Returns(true);
        //    _bookServices.Setup(s => s.GetBookById(book.Id)).Returns(bookDTO);
        //    _shelfRepo.Setup(s => s.SetCurrentBookProgress(It.IsAny<CurrentBook>())).Verifiable();
        //    _shelfRepo.Setup(r => r.AddBookToShelf(book.Id, currentShelf.Id, user.Id)).Returns(true);

        //    //Act
        //    bool isBookAdded = _shelfServices.AddBookToShelf(book.Id, currentShelf.Id);

        //    //Assert
        //    Assert.IsTrue(isBookAdded);
        //    _goalServices.Verify(r => r.UpdateGoal(It.IsAny<GoalDTO>()), Times.Once);
        //    _shelfRepo.Verify(r => r.SetCurrentBookProgress(It.IsAny<CurrentBook>()), Times.Once);
        //}

        //[TestMethod]
        //public void UpdateBookProgress_ShouldReturnTrue_WhenProgressIsBetweenZeroAndMaxPages()
        //{
        //    //Arrange
        //    int progress = 49;
        //    User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
        //    UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
        //    CurrentBookDTO bookDTO = new CurrentBookDTO(1, "image", "New Title", "Author", "Updated", "123", Genre.Mystery, 250, 0, Status.Not_started, userDTO);
        //    _userServices.Setup(s => s.LoadUser()).Returns(userDTO);
        //    _shelfRepo.Setup(r => r.SaveCurrentBookProgress(It.IsAny<CurrentBook>())).Returns(true);

        //    //Act
        //    bool isProgressUpdated = _shelfServices.UpdateBookProgress(bookDTO, progress);

        //    //Assert
        //    Assert.IsTrue(isProgressUpdated);
        //}

        //[TestMethod]
        //public void UpdateBookProgress_ShouldReturnFalse_WhenProgressIsLowerThanZero()
        //{
        //    //Arrange
        //    int progress = -49;
        //    UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
        //    CurrentBookDTO bookDTO = new CurrentBookDTO(1,null,"New Title","Author", "Updated", "123", Genre.Mystery, 250,  0, Status.Not_started, userDTO);

        //    //Act
        //    bool isProgressUpdated = _shelfServices.UpdateBookProgress(bookDTO, progress);

        //    //Assert
        //    Assert.IsFalse(isProgressUpdated);
        //}

        //[TestMethod]
        //public void UpdateBookProgress_ShouldReturnFalse_WhenProgressIsBiggerThanMAxPages()
        //{
        //    //Arrange
        //    int bookId = 1;
        //    int progress = 300;
        //    UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
        //    CurrentBookDTO bookDTO = new CurrentBookDTO(bookId, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250, 0, Status.Not_started, user); 

        //    //Act
        //    bool isProgressUpdated = _shelfServices.UpdateBookProgress(bookDTO, progress);

        //    //Assert
        //    Assert.IsFalse(isProgressUpdated);
        //}

        //[TestMethod]
        //public void RemoveBookFromShelf_ShouldReturnTrue_WhenBookIsOnHaveReadShelf()
        //{
        //    //Arrange
        //    User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
        //    Book book = new Book(1, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 0);
        //    BookDTO bookDTO = new BookDTO(1, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 0);
        //    RegularShelf completedShelf = new RegularShelf(1, "Have Read", user, new List<Book>() { book });
        //    UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
        //    GoalDTO goal = new GoalDTO(1, new DateTime(2025, 1, 1), new DateTime(2025, 5, 6), 3, 2, Status.In_progress, userDTO);

        //    _shelfRepo.Setup(r => r.GetShelfById(completedShelf.Id)).Returns(completedShelf);
        //    _goalServices.Setup(r => r.GetNewestGoal(false)).Returns(goal);
        //    _goalServices.Setup(r => r.UpdateGoal(It.IsAny<GoalDTO>())).Verifiable();
        //    _shelfRepo.Setup(r => r.RemoveBookFromShelf(user.Id, book.Id)).Returns(true);

        //    //Act
        //    bool isRemoved = _shelfServices.RemoveBookFromShelf(book.Id, completedShelf.Id);

        //    //Assert
        //    Assert.IsTrue(isRemoved);
        //    _goalServices.Verify(r => r.UpdateGoal(goal), Times.Once);
        //}

        //[TestMethod]
        //public void RemoveBookFromShelf_ShouldReturnTrue_WhenBookIsOnCurrentlyReadingShelf()
        //{
        //    //Arrange
        //    User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
        //    Book book = new Book( 1, null, "Title", "Author", "Description", "123", Genre.Mystery,250);
        //    RegularShelf currentShelf = new RegularShelf(1, "Currently Reading", user, new List<Book>() { book});
        //    UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);

        //    _shelfRepo.Setup(r => r.GetShelfById(currentShelf.Id)).Returns(currentShelf);
        //    _shelfRepo.Setup(r => r.RemoveFromCurrentBookShelf(user.Id, book.Id)).Verifiable();
        //    _shelfRepo.Setup(r => r.RemoveBookFromShelf(user.Id, book.Id)).Returns(true);

        //    //Act
        //    bool isRemoved = _shelfServices.RemoveBookFromShelf(book.Id, currentShelf.Id);

        //    //Assert
        //    Assert.IsTrue(isRemoved);
        //    _shelfRepo.Verify(r => r.RemoveFromCurrentBookShelf(user.Id, book.Id), Times.Once);
        //}

        //[TestMethod]
        //public void RemoveBookFromShelf_ShouldReturnFalse_WhenBookIsNotOnShelf()
        //{
        //    //Arrange
        //    BookDTO book = new BookDTO(1, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250);
        //    RegularShelf completedShelf = new RegularShelf(1, "Have Read", new List<Book>());
        //    UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
        //    GoalDTO goal = new GoalDTO(1, new DateTime(2025, 1, 1), new DateTime(2025, 5, 6), 3, 2, Status.In_progress, user);

        //    _shelfRepo.Setup(r => r.GetShelfById(completedShelf.Id)).Returns(completedShelf);
        //    _shelfRepo.Setup(r => r.GetBooksFromShelf(completedShelf.Id)).Returns(new List<Book>());

        //    //Act
        //    bool isRemoved = _shelfServices.RemoveBookFromShelf(book.Id, completedShelf.Id);

        //    //Assert
        //    Assert.IsFalse(isRemoved);
        //}
    }
}

