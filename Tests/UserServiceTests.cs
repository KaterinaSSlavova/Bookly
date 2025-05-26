using Bookly.Business_logic.Services;
using Interfaces;
using Business_logic.DTOs;
using Exceptions;
using Models.Entities;
using Models.Enums;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<IPasswordHelper> _passwordHelper;
        private readonly Mock<IHttpContextAccessor> _contextAccessor;
        private readonly UserServices _userService;

        public UserServiceTests()
        {
            _userRepo = new Mock<IUserRepository>();
            _passwordHelper = new Mock<IPasswordHelper>();
            _contextAccessor = new Mock<IHttpContextAccessor>();
            _userService = new UserServices(_userRepo.Object, _passwordHelper.Object, _contextAccessor.Object);
        }

        [TestMethod]
        public void Register_ShouldExecuteMethodOnce_WhenUserIsValid()
        {
            //Arrange
            UserDTO userDTO = new UserDTO("Username", "Email", "Password");
            User user = new User("Username", "Email", "HashedPassword");
            _passwordHelper.Setup(h => h.HashPassword(userDTO.Password)).Returns(user.Password);
            _userRepo.Setup(r => r.Register(It.Is<User>(u => u.Username == userDTO.Username && u.Email == userDTO.Email && u.Password == userDTO.Password))).Verifiable();

            //Act
            _userService.Register(userDTO);

            //Assert
            _userRepo.Verify(r => r.Register(It.Is<User>(u => u.Username == userDTO.Username && u.Email == userDTO.Email && u.Password == userDTO.Password)), Times.Once);
        }

        [TestMethod]
        public void Register_ShouldThrowException_WhenUserIsNull()
        {
            //Arrange
            UserDTO user = null;
            _userRepo.Setup(r => r.Register(It.IsAny<User>()));

            //Act and Assert
            Assert.ThrowsException<NullReferenceException>(() => _userService.Register(user));
        }

        [TestMethod]
        public void LogIn_ShouldReturnTrue_WhenUserExists()
        {
            //Arrange
            UserDTO userBeforeHash = new UserDTO(1, null, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            User userAfterHash = new User(1, null, "Username", new DateTime(2000, 1, 1), "email", "hashedPass", Role.Reader);
            _userRepo.Setup(r => r.LoadUser(userBeforeHash.Username)).Returns(userAfterHash);
            _passwordHelper.Setup(ph => ph.VerifyPassword(userBeforeHash.Password, userAfterHash.Password)).Returns(true);

            //Act
            bool isLoggedIn = _userService.LogIn(userBeforeHash);

            //Assert
            Assert.IsTrue(isLoggedIn);
        }

        [TestMethod]
        public void LogIn_ShouldReturnFalse_WhenTheUserDoesNotExist()
        {
            //Arrange
            UserDTO user = new UserDTO("Username", "Pass");
            _userRepo.Setup(r => r.LoadUser(It.IsAny<string>())).Returns((User?)null);

            //Act
            bool isLoggedIn = _userService.LogIn(user);

            //Assert
            Assert.IsFalse(isLoggedIn);
        }

        [TestMethod]
        public void GetUserByUsername_ShouldReturnUserDTO_WhenUserExists()
        {
            //Arrange 
            string username = "Username";
            User user = new User(1, null, username, new DateTime(2000, 1, 1), "email", "pass", Role.Reader);
            _userRepo.Setup(r => r.LoadUser(username)).Returns(user);

            //Act
            UserDTO? loadedUser = _userService.GetUserByUsername(username);

            //Assert
            Assert.IsNotNull(loadedUser);
            Assert.AreEqual(user.Username, loadedUser.Username);
            Assert.AreEqual(user.Email, loadedUser.Email);
            Assert.AreEqual(user.Password, loadedUser.Password);
            Assert.AreEqual(user.Id, loadedUser.Id);
            Assert.AreEqual(user.BirthDate, loadedUser.BirthDate);
            Assert.AreEqual(user.Role, loadedUser.Role);
        }

        [TestMethod]
        public void GetUserByUsername_ShouldReturnNull_WhenUserDoesNotExist()
        {
            //Arrange
            string username = "Username";
            _userRepo.Setup(r => r.LoadUser(It.IsAny<string>())).Returns((User?)null);

            //Act
            UserDTO? user = _userService.GetUserByUsername(username);

            //Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void CalculateAge_ShouldReturnExpectedAge_WhenUserHasValidBirthDate()
        {
            //Arrange
            int expectedAge = 24;
            User user = new User(1, null, "Username", new DateTime(2000, 12, 1), "email", "pass", Role.Reader);

            //Act
            int resultAge = _userService.CalculateAge(user);

            //Assert
            Assert.AreEqual(expectedAge, resultAge);
        }

        [TestMethod]
        public void CalculateAge_ShouldReturnZero_WhenBirthDateIsNull()
        {
            //Arrange
            int expectedAge = 0;
            User user = new User(1, null, "Username", null, "email", "pass", Role.Reader);

            //Act
            int resultAge = _userService.CalculateAge(user);

            //Assert
            Assert.AreEqual(expectedAge, resultAge);
        }

        [TestMethod]
        public void UpdateProfile_ShouldUpdateTheUser_WhenUserIsValid()
        {
            //Arrange
            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2001, 2, 2), 25, "email", "Pass", Role.Reader);
            User oldUserVersion = new User(1, null, "Username", new DateTime(2001, 1, 1), "email", "Pass", Role.Reader);

            _userRepo.Setup(r => r.LoadUser(oldUserVersion.Username)).Returns(oldUserVersion);
            _userRepo.Setup(r => r.DoesUsernameExists(It.IsAny<User>())).Returns(false);
            _userRepo.Setup(r => r.DoesEmailExists(It.IsAny<User>())).Returns(false);
            _userRepo.Setup(r => r.UpdateProfile(It.IsAny<User>()));

            //Act
            _userService.UpdateProfile(userDTO);

            //Assert
            _userRepo.Verify(r => r.UpdateProfile(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        public void UpdateProfile_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            //Arrange
            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2001, 2, 2), 25, "email", "Pass", Role.Reader);
            User oldUserVersion = new User(1, null, "Username", new DateTime(2001, 1, 1), "email", "Pass", Role.Reader);

            _userRepo.Setup(r => r.LoadUser(oldUserVersion.Username)).Returns(oldUserVersion);
            _userRepo.Setup(r => r.DoesUsernameExists(It.IsAny<User>())).Returns(true);

            //Act and Assert
            Assert.ThrowsException<UsernameAlreadyExistsException>(() => _userService.UpdateProfile(userDTO));
        }

        [TestMethod]
        public void UpdateProfile_ShouldThrowException_WhenEmailAlreadyExists()
        {
            //Arrange
            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2001, 2, 2), 25, "email", "Pass", Role.Reader);
            User oldUserVersion = new User(1, null, "Username", new DateTime(2001, 1, 1), "email", "Pass", Role.Reader);

            _userRepo.Setup(r => r.LoadUser(oldUserVersion.Username)).Returns(oldUserVersion);
            _userRepo.Setup(r => r.DoesUsernameExists(It.IsAny<User>())).Returns(false);
            _userRepo.Setup(r => r.DoesEmailExists(It.IsAny<User>())).Returns(true);

            //Act and Assert
            Assert.ThrowsException<EmailAlreadyExistsException>(() => _userService.UpdateProfile(userDTO));
        }

        [TestMethod]
        public void UpdateProfile_ShouldThrowException_WhenUserBirthDateNotValid()
        {
            //Arrange
            User user = new User(1, null, "Username", null, "email", "pass", Role.Reader);
            string pictureDTO = Convert.ToBase64String(new byte[] { 0x01, 0x02, 0x03 });
            UserDTO userDTO = new UserDTO(1, pictureDTO, "Username", new DateTime(2030, 2, 2), null, "email", "Pass", Role.Reader);

            _userRepo.Setup(r => r.LoadUser(user.Username)).Returns(user);

            //Act and Assert
            Assert.ThrowsException<InvalidBirthdayException>(() => _userService.UpdateProfile(userDTO));
        }

        [TestMethod]
        public void UpdateProfile_ShouldThrowException_WhenUserIsNull()
        {
            //Arrange
            User user = new User(1, null, "Username", null, "email", "pass", Role.Reader);
            UserDTO userDTO = null;

            _userRepo.Setup(r => r.LoadUser(user.Username)).Returns(user);

            //Act and Assert
            Assert.ThrowsException<NullReferenceException>(() => _userService.UpdateProfile(userDTO));
        }

    }
}
