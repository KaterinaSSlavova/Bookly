using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using ViewModels.Model;
using Bookly.Business_logic.InterfacesServices;

namespace Bookly.Bookly.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _iuserService;
        private readonly IShelfServices _shelfService;
        public UserController(IUserServices iuserServices, IShelfServices shelfService)
        {
            this._iuserService = iuserServices;
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
                if (_iuserService.Register(user))
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
            User? loggedUser = _iuserService.LogIn(model);
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
            User? user = _iuserService.LoadUser();
            return View(user);
        }

        [HttpPost]
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveChanges(IFormFile picture, string username, int age, string email, string password)
        {
            User? user = _iuserService.LoadUser();
            if(_iuserService.UpdateProfile(user, picture, username, age, email,password))
            {
                return RedirectToAction("ViewProfile", "User");
            }
            ViewBag.ErrorMessage="Profile was not updated!";
            return RedirectToAction("EditProfile", "User");
        }
    }
}
