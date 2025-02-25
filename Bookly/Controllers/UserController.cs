using Microsoft.AspNetCore.Mvc;
using Bookly.Models;
using Bookly.Repository;

namespace Bookly.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if(DbHelper.Register(user))
            {
                return RedirectToAction("LogIn");
            }
            else
            {
                ViewBag.ErrorMessage = "Username or email already taken!";
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(string username, string password)
        {
            User loggedUser=DbHelper.LogIn(username, password);
            if(loggedUser!=null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password! Please try again!";
                return View(loggedUser);
            }
        }
    }
}
