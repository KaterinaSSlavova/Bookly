using Microsoft.AspNetCore.Mvc;
using Bookly.Models;
using Bookly.Repository;
using Bookly.ViewModels;

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
        public IActionResult Register(AccountRegister user)
        {
            try
            {
                if (DbHelper.Register(user))
                {
                    return RedirectToAction("LogIn");
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
            User loggedUser=DbHelper.LogIn(username, password);
            if(loggedUser!=null)
            {
                HttpContext.Session.SetString("Username", loggedUser.Username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password! Please try again!";
                return View(loggedUser);
            }
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LogIn","User");
        }
    }
}
