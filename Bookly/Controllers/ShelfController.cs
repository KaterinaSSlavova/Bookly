using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;
using ViewModels.Model;
using AutoMapper;

namespace Bookly.Bookly.Controllers
{
    public class ShelfController : Controller
    {
        private readonly IShelfServices _ishelfService;
        private readonly IUserServices _iuserService;
        private readonly IMapper _mapper;
        public ShelfController(IShelfServices ishelfService, IUserServices iuserService, IMapper mapper)
        {
            this._ishelfService = ishelfService;
            this._iuserService = iuserService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ShelfOverview()
        {
            List<Shelf> myShelves = _ishelfService.GetUserShelves();
            List<ShelfViewModel> model = _mapper.Map<List<ShelfViewModel>>(myShelves);    
            return View(model);
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
            Shelf shelf = _mapper.Map<Shelf>(shelfModel);   
            if (!_ishelfService.CreateShelf(shelf)) 
            {
                TempData["ShelfError"] = "Invalid data! Shelf name must be unique!";
                return RedirectToAction("CreateShelf", "Shelf");
            }
            return RedirectToAction("ShelfOverview", "Shelf");
        }


        [HttpGet]
        public IActionResult ShelfDetails(int id)
        {
            Shelf shelf=_ishelfService.GetShelfById(id);
            List<Book> books = _ishelfService.GetBooksFromShelf(id);
            ShelfViewModel shelfModel = _mapper.Map<ShelfViewModel>(shelf);
            shelfModel.BooksOnShelf = _mapper.Map<List<BookViewModel>>(books);
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
        public IActionResult MoveToShelf(int bookId, int shelfId)
        {
            if (_ishelfService.AddBookToShelf(bookId, shelfId))
            {
                TempData["Message"] = "Book added to shelf!";
            }
            else
            {
                TempData["Warning"] = "This book is already placed on that shelf!";
            }
            return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
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
