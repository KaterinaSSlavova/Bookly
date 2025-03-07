using Microsoft.AspNetCore.Mvc;
using Bookly.Models;
using Bookly.Repository;

namespace Bookly.Controllers
{
    public class ShelfController : Controller
    {
        [HttpGet]
        public IActionResult ShelfOverview()
        {
            string? username = HttpContext.Session.GetString("Username");
            ViewBag.Username =username;
            User? user = DbHelper.LoadUser(username);
            List<Shelf> myShelves = DbHelper.GetUserShelves(user.Id);
            return View(myShelves);
        }

        [HttpGet]
        public IActionResult CreateShelf()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }

        [HttpPost]
        public IActionResult GoToCreatePage()
        {
            return RedirectToAction("CreateShelf", "Shelf");
        }

        [HttpPost]
        public IActionResult CreateNewShelf(Shelf shelf)
        {
            string? username = HttpContext.Session.GetString("Username");
            User? user = DbHelper.LoadUser(username);
            if (!DbHelper.CreateShelf(shelf.Name, user.Id))
            {
                ViewBag.ErrorMessage = "The shelf was not created!";
            }
            return RedirectToAction("ShelfOverview", "Shelf");
        }


        [HttpGet]
        public IActionResult ShelfDetails(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            Shelf? shelf=DbHelper.GetShelfById(id);
            shelf.Books = DbHelper.GetBooksFromShelf(id);
            return View(shelf);
        } 

        [HttpPost]
        public IActionResult ViewShelf(int id)
        {
            return RedirectToAction("ShelfDetails", "Shelf", new { id = id });
        }

        [HttpPost]
        public IActionResult GoBack()
        {
            return RedirectToAction("ShelfOverview", "Shelf");
        }
    }
}
