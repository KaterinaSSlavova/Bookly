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
                TempData["ShelfError"] = "Invalid data! Shelf name must be unique!";
                return RedirectToAction("CreateShelf", "Shelf");
            }
            return RedirectToAction("ShelfOverview", "Shelf");
        }


        [HttpGet]
        public IActionResult ShelfDetails(int id)
        {
            ShelfViewModel shelfModel=_ishelfService.GetShelfById(id);
            shelfModel.BooksOnShelf = _ishelfService.GetBooksOnShelfModel(id);
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
            if(!_ishelfService.RemoveBookFromShelf(bookId, shelfId))
            {
                TempData["RemoveBookError"] = "Book cannot be removed!";
            }
            else
            {
                TempData["RemoveBookSuccess"] = "Book was removed successfully!";
            }
            return RedirectToAction("ShelfDetails", "Shelf", new { id=shelfId });
        }

        [HttpPost]
        public IActionResult RemoveShelf(int id)
        {
            if(!_ishelfService.RemoveShelf(id))
            {
                TempData["ShelfError"] = "Shelf cannot be removed!";
            }
            else
            {
                TempData["ShelfSuccess"] = "Shelf was removed successfully!";
            }
            return RedirectToAction("ShelfOverview","Shelf");
        }
    }
}
