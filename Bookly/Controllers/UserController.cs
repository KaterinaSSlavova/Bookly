using Microsoft.AspNetCore.Mvc;
using ViewModels.Model;
using Bookly.Business_logic.InterfacesServices;
using Business_logic.DTOs;
using AutoMapper;
using Bookly.Business_logic.Services;

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
                UserDTO user = _mapper.Map<UserDTO>(model);
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
            UserDTO user = _mapper.Map<UserDTO>(model);
            if (_userService.LogIn(user))
            {
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Index", "Book");
            }
            ViewBag.ErrorMessage = "Invalid username or password! Please try again!";
            return View(user);
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
            UserDTO? user = _userService.LoadUser();
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
            UserDTO user = _mapper.Map<UserDTO>(model);
            if(_userService.UpdateProfile(user, model.Picture))
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
