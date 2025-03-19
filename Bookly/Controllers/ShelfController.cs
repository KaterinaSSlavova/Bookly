using Microsoft.AspNetCore.Mvc;
using Bookly.Data.Models;
using Bookly.Business_logic.InterfacesServices;

namespace Bookly.Bookly.Controllers
{
    public class ShelfController : Controller
    {
        private readonly IShelfServices _ishelfService;
        private readonly IUserServices _iuserService;
        public ShelfController(IShelfServices ishelfService, IUserServices iuserService)
        {
            this._ishelfService = ishelfService;
            this._iuserService = iuserService;
        }

        [HttpGet]
        public IActionResult ShelfOverview()
        {
            string? username = HttpContext.Session.GetString("Username");
            ViewBag.Username =username;
            User? user = _iuserService.LoadUser(username);
            List<Shelf> myShelves = _ishelfService.GetUserShelves(user.Id);
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
            User? user = _iuserService.LoadUser(username);
            if (!_ishelfService.CreateShelf(shelf.Name, user.Id)) 
            {
                ViewBag.ErrorMessage = "The shelf was not created!";
            }
            return RedirectToAction("ShelfOverview", "Shelf");
        }


        [HttpGet]
        public IActionResult ShelfDetails(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            Shelf? shelf=_ishelfService.GetShelfById(id);
            shelf.Books = _ishelfService.GetBooksFromShelf(id);
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
            User? user = _iuserService.LoadUser(HttpContext.Session.GetString("Username"));
            if(_ishelfService.RemoveBookFromShelf(user.Id, bookId, shelfId))
            {
                TempData["Confirmation"] = "Book was removed successfully!";
            }
            return RedirectToAction("ShelfDetails", "Shelf", new {id=shelfId});
        }

        [HttpPost]
        public IActionResult RemoveShelf(int id)
        {
            _ishelfService.RemoveShelf(id);
            return RedirectToAction("ShelfOverview","Shelf");
        }
    }
}
