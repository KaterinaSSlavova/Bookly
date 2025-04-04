using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using ViewModels.Model;
using Bookly.Business_logic.InterfacesServices;

namespace Bookly.Bookly.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userService;
        private readonly IShelfServices _shelfService;
        public UserController(IUserServices userServices, IShelfServices shelfService)
        {
            _userService = userServices;
            _shelfService = shelfService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountRegister user)
        {
            try
            {
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
            return View(user);
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(AccountLogIn model)
        {
            User? loggedUser = _userService.LogIn(model);
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
            ProfileOverviewModel? user = _userService.LoadProfile();
            return View(user);
        }

        [HttpPost]
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveChanges(EditProfileModel editModel)
        {
            if(_userService.UpdateProfile(editModel))
            {
                return RedirectToAction("ViewProfile", "User");
            }
            ViewBag.ErrorMessage="Profile was not updated!";
            return RedirectToAction("EditProfile", "User");
        }
    }
}
