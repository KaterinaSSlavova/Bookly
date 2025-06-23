//using AutoMapper;
//using Interfaces;
//using Bookly.Business_logic.Services;
//using Business_logic.DTOs;
//using Exceptions;
//using Business_logic.Mappers;
//using Models.Entities;
//using Models.Enums;
//using Moq;

//namespace Tests
//{
//    [TestClass]
//    public class ShelfServiceTests
//    {
//        private readonly Mock<IShelfRepository> _shelfRepo;
//        private readonly Mock<IBookServices> _bookServices;
//        private readonly Mock<IGoalServices> _goalServices;
//        private readonly Mock<IUserServices> _userServices;
//        private readonly IMapper _mapper;
//        private readonly ShelfServices _shelfServices;

//        public ShelfServiceTests()
//        {
//            _shelfRepo = new Mock<IShelfRepository>();
//            _bookServices = new Mock<IBookServices>();
//            _goalServices = new Mock<IGoalServices>();
//            _userServices = new Mock<IUserServices>();
//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.AddProfile(new ShelfMapper());
//                cfg.AddProfile(new BookMapper());
//                cfg.AddProfile(new UserMapper());
//            });
//            _mapper = config.CreateMapper();

//            _shelfServices = new ShelfServices(_shelfRepo.Object, _mapper, _userServices.Object, _goalServices.Object, _bookServices.Object);
//        }

//        [TestMethod]
//        public void CreateShelf_ShouldExecuteRepoUpdate_WhenShelfIsValid()
//        {
//            //Arrange
//            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
//            User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
//            List<RegularShelf> shelves = new List<RegularShelf>();
//            _userServices.Setup(s => s.LoadUser()).Returns(userDTO);
//            _userServices.Setup(s => s.ConvertToEntity(userDTO)).Returns(user);
//            _shelfRepo.Setup(r => r.GetUserRegularShelves(user)).Returns(shelves);
//            _shelfRepo.Setup(r => r.CreateShelf(It.IsAny<Shelf>(), user.Id)).Verifiable();

//            //Act
//            _shelfServices.CreateShelf(new ShelfDTO("Shelf"));

//            //Assert
//            _shelfRepo.Verify(r => r.CreateShelf(It.IsAny<Shelf>(), user.Id));
//        }

//        [TestMethod]
//        public void CreateShelf_ShouldThrowException_WhenShelfNull()
//        {
//            //Arrange
//            User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
//            ShelfDTO? shelfDTO = null;
//            _shelfRepo.Setup(r => r.GetUserRegularShelves(user)).Returns(It.IsAny<List<RegularShelf>>());

//            //Act and Assert
//            Assert.ThrowsException<NullReferenceException>(() => _shelfServices.CreateShelf(shelfDTO));
//        }

//        [TestMethod]
//        public void CreateShelf_ShouldThrowException_WhenShelfWithTheSameNameAlreadyExists()
//        {
//            //Arrange
//            User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
//            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
//            ShelfDTO shelfDTO = new ShelfDTO("Shelf");
//            List<RegularShelf> shelves = new List<RegularShelf>() { new RegularShelf( new Shelf(1, "Shelf"), new List<Book>()) };

//            _userServices.Setup(r => r.ConvertToEntity(It.IsAny<UserDTO>())).Returns(user);
//            _userServices.Setup(r => r.LoadUser()).Returns(userDTO);
//            _shelfRepo.Setup(r => r.GetUserRegularShelves(user)).Returns(shelves);

//            //Act and Assert
//            Assert.ThrowsException<ShelfAlreadyExistsException>(() => _shelfServices.CreateShelf(shelfDTO));
//        }


//        [TestMethod]
//        public void RemoveShelf_ShouldExecuteMethodOnce_WhenShelfIdIsNotZero()
//        {
//            //Arrange
//            int shelfId = 1;
//            _shelfRepo.Setup(r => r.RemoveShelf(shelfId)).Verifiable();

//            //Act
//            _shelfServices.RemoveShelf(shelfId);

//            //Assert
//            _shelfRepo.Verify(r => r.RemoveShelf(shelfId), Times.Once);
//        }

//        [TestMethod]
//        public void RemoveShelf_ShouldThrowException_WhenShelfIdIsNegative()
//        {
//            //Arrange
//            int shelfId = -1;

//            //Act and Assert
//            Assert.ThrowsException<NullReferenceException>(() => _shelfServices.RemoveShelf(shelfId));
//        }

//        [TestMethod]
//        public void AddBookToShelf_ShouldExecuteMethods_WhenBookIsNotOnTheShelf()
//        {
//            //Arrange
//            User user = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);
//            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
//            RegularShelfDTO shelfDTO = new RegularShelfDTO(new ShelfDTO(1, "Have Read", userDTO));
//            BookDTO book = new BookDTO(3, null, "New Title", "Author", "Description", "123", Genre.Mystery, 250);

//            _shelfRepo.Setup(r => r.GetUserRegularShelves(user)).Returns(It.IsAny<List<RegularShelf>>());
//            _goalServices.Setup(s => s.IncreaseProgress()).Verifiable();
//            _shelfRepo.Setup(r => r.AddBookToShelf(book.Id, shelfDTO.Shelf.Id, user.Id)).Verifiable();

//            //Act 
//            _shelfServices.AddBookToShelf(book, shelfDTO);

//            //Assert
//            _shelfRepo.Verify(r => r.AddBookToShelf(book.Id, shelfDTO.Shelf.Id, user.Id), Times.Once);
//            _goalServices.Verify(s => s.IncreaseProgress(), Times.Once);
//        }

//        [TestMethod]
//        public void AddBookToShelf_ShouldThrowException_WhenBookIsOnShelf()
//        {
//            //Arrange
//            RegularShelf shelf = new RegularShelf(new Shelf(1, "Have Read"), new List<Book>()
//            { new Book(3, null, "New Title", "Author", "Description", "123", Genre.Mystery, 250) });
//            RegularShelfDTO shelfDTO = new RegularShelfDTO(new ShelfDTO(1, "Have Read"), new List<BookDTO>()
//            { new BookDTO(3, null, "New Title", "Author", "Description", "123", Genre.Mystery, 250) });
//            BookDTO book = new BookDTO(3, null, "New Title", "Author", "Description", "123", Genre.Mystery, 250);

//            _shelfRepo.Setup(r => r.GetShelfById(shelf.Shelf.Id)).Returns(shelf);
//            _bookServices.Setup(s => s.GetBookById(book.Id)).Returns(book);

//            //Act and Assert
//            Assert.ThrowsException<BookIsAlreadyOnShelfException>(() => _shelfServices.AddBookToShelf(book, shelfDTO));
//        }

//        [TestMethod]
//        public void UpdateBookProgress_ShouldExecuteMethod_WhenProgressIsBetweenZeroAndMaxPages()
//        {
//            //Arrange
//            int progress = 49;
//            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
//            CurrentBookDTO bookDTO = new CurrentBookDTO(new BookDTO(1, "image", "New Title", "Author", "Updated", "123", Genre.Mystery, 250), userDTO);

//            _userServices.Setup(s => s.LoadUser()).Returns(userDTO);
//            _shelfRepo.Setup(r => r.SaveCurrentBookProgress(It.IsAny<CurrentBook>()));

//            //Act
//            _shelfServices.UpdateBookProgress(bookDTO, progress);

//            //Assert
//            _shelfRepo.Verify(r => r.SaveCurrentBookProgress(It.IsAny<CurrentBook>()), Times.Once);
//        }

//        [TestMethod]
//        public void UpdateBookProgress_ShouldThrowException_WhenProgressIsLowerThanZero()
//        {
//            //Arrange
//            int progress = -49;
//            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
//            CurrentBookDTO bookDTO = new CurrentBookDTO(new BookDTO(1, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250), userDTO);

//            //Act and Assert
//            Assert.ThrowsException<InvalidProgressException>(() => _shelfServices.UpdateBookProgress(bookDTO, progress));
//        }

//        [TestMethod]
//        public void UpdateBookProgress_ShouldReturnFalse_WhenProgressIsBiggerThanMAxPages()
//        {
//            //Arrange
//            int progress = 300;
//            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
//            CurrentBookDTO bookDTO = new CurrentBookDTO(new BookDTO(1, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250), user);

//            //Act and Assert
//            Assert.ThrowsException<InvalidProgressException>(() => _shelfServices.UpdateBookProgress(bookDTO, progress));
//        }

//        [TestMethod]
//        public void RemoveBookFromShelf_ShouldRemoveBookFromShelf_WhenBookIsOnCurrentlyReadingShelf()
//        {
//            //Arrange
//            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
//            BookDTO book = new BookDTO(1, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 90);
//            RegularShelfDTO currentShelf = new RegularShelfDTO(new ShelfDTO(1, "Currently Reading", user), new List<BookDTO>() { book });

//            _shelfRepo.Setup(r => r.RemoveFromCurrentBookShelf(user.Id, book.Id)).Verifiable();
//            _shelfRepo.Setup(r => r.RemoveBookFromShelf(user.Id, book.Id)).Verifiable();

//            //Act
//            _shelfServices.RemoveBookFromShelf(book.Id, currentShelf);

//            //Assert
//            _shelfRepo.Verify(r => r.RemoveFromCurrentBookShelf(user.Id, book.Id), Times.Once);
//            _shelfRepo.Verify(r => r.RemoveBookFromShelf(user.Id, book.Id), Times.Once);
//        }


//        [TestMethod]
//        public void RemoveBookFromShelf_ShouldRemoveBookFromShelf_WhenBookIsOnHaveReadShelf()
//        {
//            //Arrange
//            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
//            BookDTO bookDTO = new BookDTO(1, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 50);
//            RegularShelfDTO completedShelf = new RegularShelfDTO(new ShelfDTO(1, "Have Read", user), new List<BookDTO>() { bookDTO });

//            _goalServices.Setup(s => s.DecreaseProgress()).Verifiable();
//            _shelfRepo.Setup(r => r.RemoveBookFromShelf(user.Id, bookDTO.Id)).Verifiable();

//            //Act
//            _shelfServices.RemoveBookFromShelf(bookDTO.Id, completedShelf);

//            //Assert
//            _goalServices.Verify(r => r.DecreaseProgress(), Times.Once);
//            _shelfRepo.Verify(r => r.RemoveBookFromShelf(user.Id, bookDTO.Id), Times.Once);
//        }

//        [TestMethod]
//        public void RemoveBookFromShelf_ShouldExecuteMethodOnce_WhenBookIsOnShelf()
//        {
//            //Arrange
//            UserDTO user = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
//            BookDTO book = new BookDTO(1, null, "Title", "Author", "Description", "123", Genre.Mystery, 250);
//            RegularShelfDTO currentShelf = new RegularShelfDTO(new ShelfDTO(1, "Shelf", user), new List<BookDTO>() { book });

//            _shelfRepo.Setup(r => r.RemoveBookFromShelf(user.Id, book.Id)).Verifiable();

//            //Act
//            _shelfServices.RemoveBookFromShelf(book.Id, currentShelf);

//            //Assert
//            _shelfRepo.Verify(r => r.RemoveBookFromShelf(user.Id, book.Id), Times.Once);
//        }

//        [TestMethod]
//        public void RemoveBookFromShelf_ShouldThrowException_WhenShelfDoesNotExist()
//        {
//            //Arrange
//            BookDTO book = new BookDTO(1, null, "New Title", "Author", "Updated", "123", Genre.Mystery, 250);

//            //Act and Assert
//            Assert.ThrowsException<NullReferenceException>(() => _shelfServices.RemoveBookFromShelf(book.Id, null));
//        }
//    }
//}

