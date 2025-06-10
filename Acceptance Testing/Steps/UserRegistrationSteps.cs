using Bookly.Bookly.Controllers;
using Bookly.ViewModels;
using Interfaces;
using Moq;
using TechTalk.SpecFlow;
using AutoMapper;
using Business_logic.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Acceptance_Testing.Steps
{
    [Binding]
    public class UserRegistrationSteps
    {
        private AccountRegister _registration;
        private Mock<IUserServices> _userService;
        private Mock<IShelfServices> _shelfServices;
        private Mock<IMapper> _mapper;
        private UserController _controller;
        private UserDTO user;
        private IActionResult _actionResult;

        [Given(@"I am on the registration page")]
        public void GivenIAmOnTheRegistrationPage()
        {
            _registration = new AccountRegister();
            _userService = new Mock<IUserServices>();
            _shelfServices = new Mock<IShelfServices>();
            _mapper = new Mock<IMapper>();
            _controller = new UserController(_userService.Object, _shelfServices.Object, _mapper.Object);
        }

        [When(@"I enter a valid registration details")]
        public void WhenIEnterValidRegistrationDetails()
        {
            _registration.Username = "New User";
            _registration.Email = "user@mail.com";
            _registration.Password = "password";
            _registration.ConfirmPassword = "password";
        }

        [When(@"I click the ""Register"" button")]
        public void WhenIClickTheRegisterButton()
        {
            user = new UserDTO(_registration.Username, _registration.Email, _registration.Password);
            _mapper.Setup(m => m.Map<UserDTO>(_registration))
                .Returns(user);

            _userService.Setup(s => s.Register(user)).Verifiable();

            _actionResult = _controller.Register(_registration);
        }

        [Then(@"my account should be created")]
        public void ThenMyAccountShouldBeCreated()
        {
            _userService.Verify(s => s.Register(user), Times.Once);
        }

        [Then(@"I should be redirected to Log in page")]
        public void IShouldBeRedirectedToLogInPage()
        {
            var redirectResult = _actionResult as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(redirectResult.ActionName, "LogIn");
        }
    }
}
