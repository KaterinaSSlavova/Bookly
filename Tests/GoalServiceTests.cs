using Bookly.Business_logic.Services;
using Business_logic.DTOs;
using Interfaces;
using Moq;
using Models.Enums;
using Models.Entities;
using Exceptions;

namespace Tests
{
    [TestClass]
    public class GoalServiceTests
    {

        private readonly Mock<IGoalRepository> _goalRepo;
        private readonly Mock<IUserServices> _userService;
        private readonly Mock<IEmailService> _emailService;
        private readonly GoalServices _goalServices;
        public GoalServiceTests()
        {
            _goalRepo = new Mock<IGoalRepository>();
            _userService = new Mock<IUserServices>();
            _emailService = new Mock<IEmailService>();
            _goalServices = new GoalServices(_goalRepo.Object, _userService.Object, _emailService.Object);
        }

        [TestMethod]
        public void CreateGoal_ShouldExecuteMethod_WhenGoalIsValid()
        {
            //Arrange
            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2001, 2, 2), 25, "email", "Pass", Role.Reader);
            User user = new User(1, null, "Username", new DateTime(2001, 1, 1), "email", "Pass", Role.Reader);
            GoalDTO goalDTO = new GoalDTO(1, new DateTime(2025, 8, 1), new DateTime(2025, 9, 1), 3, 0, Status.Not_started, userDTO);

            _userService.Setup(s => s.LoadUser()).Returns(userDTO);
            _userService.Setup(s => s.ConvertToEntity(userDTO)).Returns(user);
            _goalRepo.Setup(r => r.CreateGoal(It.IsAny<Goal>())).Verifiable();

            //Act
            _goalServices.CreateGoal(goalDTO);

            //Assert
            _goalRepo.Verify(r => r.CreateGoal(It.IsAny<Goal>()), Times.Once);
        }

        [TestMethod]
        public void CreateGoal_ShouldThrowException_WhenGoalIsNull()
        {
            //Arrange
            GoalDTO goalDTO = null;

            //Act and Assert
            Assert.ThrowsException<NullReferenceException>(() => _goalServices.CreateGoal(goalDTO));
        }

        [TestMethod]
        public void CreateGoal_ShouldThrowException_WhenReadingGoalLowerOrEqualToZero()
        {
            //Arrange
            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2001, 2, 2), 25, "email", "Pass", Role.Reader);
            User user = new User(1, null, "Username", new DateTime(2001, 1, 1), "email", "Pass", Role.Reader);
            GoalDTO goalDTO = new GoalDTO(1, new DateTime(2025, 8, 1), new DateTime(2025, 9, 1), -1, 0, Status.Not_started, userDTO);

            //Act and Assert
            Assert.ThrowsException<InvalidReadingGoalException>(() => _goalServices.CreateGoal(goalDTO));
        }

        [TestMethod]
        public void CreateGoal_ShouldThrowException_WhenStartDateIsInvalid()
        {
            //Arrange
            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2001, 2, 2), 25, "email", "Pass", Role.Reader);
            User user = new User(1, null, "Username", new DateTime(2001, 1, 1), "email", "Pass", Role.Reader);
            GoalDTO goalDTO = new GoalDTO(1, new DateTime(2025, 8, 1), new DateTime(2025, 7, 1), 3, 0, Status.Not_started, userDTO);

            //Act and Assert
            Assert.ThrowsException<InvalidGoalStartDateException>(() => _goalServices.CreateGoal(goalDTO));
        }

        [TestMethod]
        public void CreateGoal_ShouldThrowException_WhenEndDateIsInvalid()
        {
            //Arrange
            UserDTO userDTO = new UserDTO(1, null, "Username", new DateTime(2001, 2, 2), 25, "email", "Pass", Role.Reader);
            User user = new User(1, null, "Username", new DateTime(2001, 1, 1), "email", "Pass", Role.Reader);
            GoalDTO goalDTO = new GoalDTO(1, new DateTime(2025, 8, 1), new DateTime(2024, 7, 1), 3, 0, Status.Not_started, userDTO);

            //Act and Assert
            Assert.ThrowsException<InvalidGoalStartDateException>(() => _goalServices.CreateGoal(goalDTO));
        }
    }
}
