using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using ViewModels.Model;
using Bookly.Business_logic.InterfacesServices;
using Business_logic.DTOs;
using AutoMapper;

namespace Bookly.Bookly.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userService;
        private readonly IShelfServices _shelfService;
        private readonly IMapper _mapper;
        public UserController(IUserServices userServices, IShelfServices shelfService, IMapper mapper)
        {
            _userService = userServices;
            _shelfService = shelfService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountRegister model)
        {
            try
            {
                User user = _mapper.Map<User>(model);
                if (_userService.Register(user))
                {
                    _shelfService.CreateDefaultShelf();
                    return RedirectToAction("LogIn", "User");
                }
            }
            catch (ApplicationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(AccountLogIn model)
        {
            User user = _mapper.Map<User>(model);
            User? loggedUser = _userService.LogIn(user);
            if (loggedUser != null)
            {
                HttpContext.Session.SetString("Username", loggedUser.Username);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Invalid username or password! Please try again!";
            return View(loggedUser);
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LogIn", "User");
        }

        [HttpGet]
        public IActionResult ViewProfile()
        {
            User user = _userService.LoadUser();
            ProfileOverviewModel? viewModel = _mapper.Map<ProfileOverviewModel>(user);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GoToEdit()
        {
            return RedirectToAction("EditProfile", "User");
        }

        [HttpPost]
        public IActionResult SaveChanges(EditProfileModel model)
        {
            UserDTO user = new UserDTO(model.Picture, model.Username, model.Age, model.Email);
            if(_userService.UpdateProfile(user))
            {
                TempData["ProfileUpdated"] = "Profile updated successfully!";
                return RedirectToAction("ViewProfile", "User");
            }
            else
            {
                TempData["ProfileError"] = "Invalid data! Username and email must be unique!";
                return RedirectToAction("EditProfile", "User");
            }
        }
    }
}
