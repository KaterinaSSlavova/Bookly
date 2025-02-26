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
            User? loggedUser = DbHelper.LogIn(username, password);
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
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }

        [HttpPost]
        public IActionResult Edit()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return RedirectToAction("EditProfile", "User");
        }

        //[HttpPost]
        //public IActionResult Settings(User loggedUser)
        //{
        //    User? user = DbHelper.LoadUser(loggedUser.Username);
        //    if(user!=null)
        //    {
        //        try
        //        {
        //            if (DbHelper.UpdateProfile(user))
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }
        //        catch(ApplicationException ex)
        //        {
        //            ViewBag.ErrorMessage = ex.Message;
        //            return View(loggedUser);
        //        }
        //    }
        //    ViewBag.ErrorMessage = "Profile not found!";
        //    return View(loggedUser);
        //}
    }
}
