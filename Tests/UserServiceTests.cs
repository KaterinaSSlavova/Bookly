using Bookly.Business_logic.Services;
using Bookly.Data.InterfacesRepo;
using Business_logic.DTOs;
using Business_logic.InterfacesHelpers;
using Microsoft.AspNetCore.Http;
using Models.Entities;
using Models.Enums;
using Moq;

namespace Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<IHttpContextAccessor> _contextAccessor;
        private readonly Mock<IPasswordHelper> _passwordHelper;
        private readonly UserServices _userService;

        public UserServiceTests()
        {
            _userRepo = new Mock<IUserRepository>();
            _contextAccessor = new Mock<IHttpContextAccessor>();
            _passwordHelper = new Mock<IPasswordHelper>();
            _userService = new UserServices(_userRepo.Object, _contextAccessor.Object, _passwordHelper.Object);
        }

        private void SetSession(string username)
        {
            byte[] usernameBytes = System.Text.Encoding.UTF8.GetBytes(username);
            Mock<ISession> session = new Mock<ISession>();
            session.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()));
            session.Setup(s => s.TryGetValue("Username", out usernameBytes)).Returns(true);

            _contextAccessor.Setup(a => a.HttpContext).Returns(new DefaultHttpContext() { Session = session.Object });
        }

        [TestMethod]
        public void Register_ShouldReturnTrue_WhenUserIsRegistered()
        {
            //Arrange
            UserDTO userDTO = new UserDTO("Username", "Email", "Password");
            string hashedPass = "HashedPassword";
            User user = new User("Username", "Email", hashedPass);
            _passwordHelper.Setup(h => h.HashPassword(userDTO.Password)).Returns(hashedPass);
            _userRepo.Setup(r => r.Register(It.Is<User>(u => u.Username == userDTO.Username && u.Email == userDTO.Email && u.Password == userDTO.Password))).Returns(true);

            //Act
            bool isRegistered = _userService.Register(userDTO);

            //Assert
            Assert.IsTrue(isRegistered);
        }

        [TestMethod]
        public void Register_ShouldReturnFalse_WhenUserIsNull()
        {
            //Arrange
            UserDTO user = null;
            _userRepo.Setup(r => r.Register(It.IsAny<User>())).Returns(false);

            //Act
            bool isRegistered = _userService.Register(user);

            //Assert
            Assert.IsFalse(isRegistered);
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
        public void UpdateProfile_ShouldExecuteMethodOnce_WhenUserIsValid()
        {
            //Arrange
            byte[] picture = new byte[] { 0x01, 0x02, 0x03 };
            string pictureDTO = Convert.ToBase64String(new byte[] { 0x01, 0x02, 0x03 });

            UserDTO userDTO = new UserDTO(1, pictureDTO, "Username1", new DateTime(2001, 2, 2), 25, "email1", "Pass1", Role.Reader);
            User oldUserVersion = new User(1, picture, "Username", new DateTime(2001, 1, 1), "email", "Pass", Role.Reader);

            SetSession(oldUserVersion.Username);

            _userRepo.Setup(r => r.GetAllEmails(It.IsAny<User>())).Returns(new List<string> { "email1@example.com", "email2@example.com" });
            _userRepo.Setup(r => r.GetAllUsernames(It.IsAny<User>())).Returns(new List<string> { "Username2", "Username3" });
            _userRepo.Setup(r => r.LoadUser(oldUserVersion.Username)).Returns(oldUserVersion);
            _userRepo.Setup(r => r.UpdateProfile
            (It.Is<User>(u => u.Id == userDTO.Id && u.Username==userDTO.Username &&
            u.BirthDate==userDTO.BirthDate && u.Email==userDTO.Email && u.Password==userDTO.Password 
            && u.Role==userDTO.Role))).Verifiable();

            //Act
            _userService.UpdateProfile(userDTO, pictureDTO);

            //Assert     
            _userRepo.Verify(r => r.UpdateProfile(It.Is<User>(u => u.Id == userDTO.Id && u.Username == userDTO.Username &&
            u.BirthDate == userDTO.BirthDate && u.Email == userDTO.Email && u.Password == userDTO.Password
            && u.Role == userDTO.Role)), Times.Once);
        }

        [TestMethod]
        public void UpdateProfile_ShouldThrowException_WhenUsernameOrEmailAlreadyExists()
        {
            //Arrange
            byte[] picture = new byte[] { 0x01, 0x02, 0x03 };
            string pictureDTO = Convert.ToBase64String(new byte[] { 0x01, 0x02, 0x03 });

            UserDTO userDTO = new UserDTO(1, pictureDTO, "Username", new DateTime(2001, 2, 2), 25, "email", "Pass", Role.Reader);
            User oldUserVersion = new User(1, picture, "Username", new DateTime(2001, 1, 1), "email", "Pass", Role.Reader);
            SetSession(oldUserVersion.Username);

            _userRepo.Setup(r => r.GetAllEmails(It.IsAny<User>())).Returns(new List<string> { "email", "email2@example.com" });
            _userRepo.Setup(r => r.GetAllUsernames(It.IsAny<User>())).Returns(new List<string> { "Username", "Username1" });
            _userRepo.Setup(r => r.LoadUser(oldUserVersion.Username)).Returns(oldUserVersion);

            //Act and Assert
            Assert.ThrowsException<ArgumentException>(() => _userService.UpdateProfile(userDTO, pictureDTO));
        }

        [TestMethod]
        public void UpdateProfile_ShouldThrowException_WhenUserBirthDateNotValid()
        {
            //Arrange
            string pictureDTO = Convert.ToBase64String(new byte[] { 0x01, 0x02, 0x03 });
            UserDTO userDTO = new UserDTO(1, pictureDTO, "Username", new DateTime(2030, 2, 2), null, "email", "Pass", Role.Reader);

            //Act and Assert
            Assert.ThrowsException<ArgumentException>(() => _userService.UpdateProfile(userDTO, pictureDTO));
        }

        [TestMethod]
        public void UpdateProfile_ShouldThrowException_WhenUserIsNull()
        {
            //Arrange
            string picture = null;
            UserDTO userDTO = null;

            //Act and Assert
            Assert.ThrowsException<ArgumentNullException>(() => _userService.UpdateProfile(userDTO, picture));
        }
    }
}
