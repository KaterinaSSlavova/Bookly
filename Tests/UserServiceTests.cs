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

        //[TestMethod]
        //public void LoadUser_ShouldReturnUserDTO_WhenUserExists()
        //{
        //    //Arrange
        //    string username = "Username";
        //    User expectedEntity = new User(1, null, username, new DateTime(2000, 1, 1), "email", "hashedPass", Role.Reader);
        //    UserDTO expectedDTO = new UserDTO(1, null, username, new DateTime(2000, 1, 1), 25, "email", "hashedPass", Role.Reader);

        //   Mock<ISession> session = new Mock<ISession>();
        //    session.Setup(s => s.Get("Username")).Returns(Encoding.UTF8.GetBytes(username));
        //    var httpContext = new Mock<HttpContext>();
        //    httpContext.Setup(c => c.Session).Returns(session.Object);
        //    _contextAccessor.Setup(ca => ca.HttpContext).Returns(httpContext.Object);

        //    //Act
        //    UserDTO userDTO = _userService.LoadUser();

        //    //Assert
        //    Assert.IsNotNull(userDTO);
        //    Assert.AreEqual(expectedDTO.Username, userDTO.Username);
        //    Assert.AreEqual(expectedDTO.BirthDate,userDTO.BirthDate);
        //    Assert.AreEqual(expectedDTO.Email, userDTO.Email);
        //    Assert.AreEqual(expectedDTO.Password, userDTO.Password);
        //    Assert.AreEqual(expectedDTO.Role, userDTO.Role);
        //}

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
        public void UpdateProfile_ShouldReturnTrue_WhenUserIsValid()
        {
            //Arrange
            byte[] picture = new byte[] { 0x01, 0x02, 0x03 };
            string pictureDTO = Convert.ToBase64String(new byte[] { 0x01, 0x02, 0x03 });

            UserDTO expectedDTO = new UserDTO(1, pictureDTO, "Username", new DateTime(2000, 1, 1), 25, "email", "Pass", Role.Reader);
            User user = new User(1, picture, "Username", new DateTime(2000, 1, 1), "email", "Pass", Role.Reader);

            byte[] usernameBytes = System.Text.Encoding.UTF8.GetBytes(user.Username);
            Mock<ISession> session = new Mock<ISession>();
            session.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()));
            session.Setup(s => s.TryGetValue("Username", out usernameBytes)).Returns(true);

            _contextAccessor.Setup(a => a.HttpContext).Returns(new DefaultHttpContext() { Session = session.Object});

            _userRepo.Setup(r => r.GetAllEmails(It.IsAny<User>())).Returns(new List<string>());
            _userRepo.Setup(r => r.GetAllUsernames(It.IsAny<User>())).Returns(new List<string>());
            _userRepo.Setup(r => r.LoadUser(user.Username)).Returns(user);
            _userRepo.Setup(r => r.UpdateProfile(It.Is<User>(u =>
                                  u.Id == user.Id &&
                                  u.Username == user.Username &&
                                  u.Email == user.Email &&
                                  u.Password == user.Password &&
                                  u.Role == user.Role &&
                                  u.BirthDate == user.BirthDate &&
                                  u.Picture.SequenceEqual(user.Picture)
                            ))).Returns(true);


            //Act
            bool isUpdated = _userService.UpdateProfile(expectedDTO, pictureDTO);

            //Assert
            Assert.IsTrue(isUpdated);
        }
    }
}
