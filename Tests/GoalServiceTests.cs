using Bookly.Business_logic.Services;
using Interfaces;
using Moq;

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

        }
    }
}
