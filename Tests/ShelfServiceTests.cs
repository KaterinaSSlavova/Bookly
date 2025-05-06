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


    }
}
