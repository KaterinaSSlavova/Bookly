using Microsoft.AspNetCore.Mvc;
using Bookly.Data.Models;
using Bookly.Data.ViewModels;
using Bookly.Business_logic.InterfacesServices;

namespace Bookly.Bookly.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _iuserService;
        public UserController(IUserServices iuserServices)
        {
            this._iuserService = iuserServices;
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
        public IActionResult LogIn(string username, string password)
        {
            User? loggedUser = _iuserService.LogIn(username, password);
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
            string? currentUser = HttpContext.Session.GetString("Username");
            ViewBag.Username=currentUser;
            User? user = _iuserService.LoadUser(currentUser);
            return View(user);
        }

        [HttpPost]
        public IActionResult EditProfile()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }

        [HttpPost]
        public IActionResult SaveChanges(IFormFile picture, string username, int age, string email, string password)
        {
            string? currentUsername= HttpContext.Session.GetString("Username");
            User? user = _iuserService.LoadUser(currentUsername);
            if(_iuserService.UpdateProfile(user, picture, username, age, email,password))
            {
                return RedirectToAction("ViewProfile", "User");
            }
            ViewBag.ErrorMessage="Profile was not updated!";
            return RedirectToAction("EditProfile", "User");
        }
    }
}
