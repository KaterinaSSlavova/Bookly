using Microsoft.AspNetCore.Mvc;
using Bookly.Models;
using Bookly.Repository;
using Bookly.Services;

namespace Bookly.Controllers
{
    public class ShelfController : Controller
    {
        private readonly ShelfServices _shelfService;
        private readonly UserServices _userService;
        public ShelfController(ShelfServices shelfService, UserServices userService)
        {
            this._shelfService = shelfService;
            this._userService = userService;
        }

        [HttpGet]
        public IActionResult ShelfOverview()
        {
            string? username = HttpContext.Session.GetString("Username");
            ViewBag.Username =username;
            User? user = _userService.LoadUser(username);
            List<Shelf> myShelves = _shelfService.GetUserShelves(user.Id);
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
            User? user = _userService.LoadUser(username);
            if (!_shelfService.CreateShelf(shelf.Name, user.Id)) 
            {
                ViewBag.ErrorMessage = "The shelf was not created!";
            }
            return RedirectToAction("ShelfOverview", "Shelf");
        }


        [HttpGet]
        public IActionResult ShelfDetails(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            Shelf? shelf=_shelfService.GetShelfById(id);
            shelf.Books = _shelfService.GetBooksFromShelf(id);
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

        [HttpPost]
        public IActionResult RemoveFromShelf(int bookId, int shelfId)
        {
            User? user = _userService.LoadUser(HttpContext.Session.GetString("Username"));
            if(_shelfService.RemoveBookFromShelf(user.Id, bookId))
            {
                TempData["Confirmation"] = "Book was removed successfully!";
            }
            return RedirectToAction("ShelfDetails", "Shelf", new {id=shelfId});
        }

        [HttpPost]
        public IActionResult RemoveShelf(int id)
        {
            _shelfService.RemoveShelf(id);
            return RedirectToAction("ShelfOverview","Shelf");
        }
    }
}
