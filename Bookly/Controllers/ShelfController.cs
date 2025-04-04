using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;
using ViewModels.Model;

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
            List<ShelfViewModel> myShelves = _ishelfService.GetUserShelfModel();
            return View(myShelves);
        }

        [HttpGet]
        public IActionResult CreateShelf()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult GoToCreatePage()
        {
            return RedirectToAction("CreateShelf", "Shelf");
        }

        [HttpPost]
        public IActionResult CreateNewShelf(ShelfViewModel shelfModel)
        {
            if (!_ishelfService.CreateShelf(shelfModel)) 
            {
                ViewBag.ErrorMessage = "The shelf was not created!";
            }
            return RedirectToAction("ShelfOverview", "Shelf");
        }


        [HttpGet]
        public IActionResult ShelfDetails(int id)
        {
            ShelfViewModel shelfModel=_ishelfService.GetShelfById(id);
            shelfModel.BooksOnShelf = _ishelfService.GetBooksFromShelf(id);
            return View(shelfModel);
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
            User? user = _iuserService.LoadUser();
            if(_ishelfService.RemoveBookFromShelf(bookId, shelfId))
            {
                TempData["Confirmation"] = "Book was removed successfully!";
            }
            return RedirectToAction("ShelfDetails", "Shelf", new { id=shelfId });
        }

        [HttpPost]
        public IActionResult RemoveShelf(int id)
        {
            _ishelfService.RemoveShelf(id);
            return RedirectToAction("ShelfOverview","Shelf");
        }
    }
}
