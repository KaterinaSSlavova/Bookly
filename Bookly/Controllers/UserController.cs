using Microsoft.AspNetCore.Mvc;
using Bookly.Models;
using Bookly.Repository;
using Bookly.ViewModels;
using System.Reflection;
using Bookly.Services;

namespace Bookly.Controllers
{
    public class UserController : Controller
    {
        private readonly UserServices _userService;
        public UserController(UserServices userServices)
        {
            this._userService = userServices;
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
            User? loggedUser = _userService.LogIn(username, password);
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
            User? user = _userService.LoadUser(currentUser);
            return View(user);
        }

        [HttpPost]
        public IActionResult EditProfile()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }

        [HttpPost]
        public IActionResult SaveChanges(string picture, string username, int age, string email, string password)
        {
            string? currentUsername= HttpContext.Session.GetString("Username");
            User? user = _userService.LoadUser(currentUsername);
            if(_userService.UpdateProfile(user, picture, username, age, email,password))
            {
                return RedirectToAction("ViewProfile", "User");
            }
            ViewBag.ErrorMessage="Profile was not updated!";
            return RedirectToAction("EditProfile", "User");
        }
    }
}
