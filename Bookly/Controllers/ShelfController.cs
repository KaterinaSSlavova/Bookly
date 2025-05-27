using AutoMapper;
using Interfaces;
using Bookly.ViewModels;
using Business_logic.DTOs;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Bookly.Filters;

namespace Bookly.Bookly.Controllers
{
    public class ShelfController : Controller
    {
        private readonly IShelfServices _shelfService;
        private readonly IMapper _mapper;
        public ShelfController(IShelfServices shelfService, IMapper mapper)
        {
            _shelfService = shelfService;
            _mapper = mapper;
        }

        [FilterLoggedUsers]
        [HttpGet]
        public IActionResult ShelfOverview()
        {
            List<RegularShelfDTO> myShelves = _shelfService.GetUserShelves();
            List<RegularShelfViewModel> model = _mapper.Map<List<RegularShelfViewModel>>(myShelves);
            return View(model);
        }

        [FilterLoggedUsers]
        [HttpGet]
        public IActionResult CreateShelf()
        {
            return View();
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult GoToCreatePage()
        {
            return RedirectToAction("CreateShelf", "Shelf");
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult CreateNewShelf(ShelfViewModel shelfModel)
        {
            if (shelfModel.Name == null)
            {
                TempData["Error"] = "Please fill all fields!";
                return RedirectToAction("CreateShelf", "Shelf");
            }

            try
            {
                ShelfDTO shelf = _mapper.Map<ShelfDTO>(shelfModel);
                _shelfService.CreateShelf(shelf);
                return RedirectToAction("ShelfOverview", "Shelf");
            }
            catch (NullReferenceException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (ShelfAlreadyExistsException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("CreateShelf", "Shelf");
        }

        [FilterLoggedUsers]
        [HttpGet]
        public IActionResult ShelfDetails(int id)
        {
            RegularShelfDTO shelf = _shelfService.GetShelfById(id);
            RegularShelfViewModel shelfModel = _mapper.Map<RegularShelfViewModel>(shelf);
            return View(shelfModel);
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult ViewShelf(int id)
        {
            return RedirectToAction("ShelfDetails", "Shelf", new { id = id });
        }

        [FilterLoggedUsers]
        [HttpGet]
        public IActionResult CurrentlyReadingOverview()
        {
            CurrentBookShelfDTO currentBooksShelf = _shelfService.GetCurrentlyReadingShelf();
            CurrentBookShelfViewModel shelfModel = _mapper.Map<CurrentBookShelfViewModel>(currentBooksShelf);
            return View(shelfModel);
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult ViewCurrentlyReadingShelf()
        {
            return RedirectToAction("CurrentlyReadingOverview", "Shelf");
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult GoBack()
        {
            return RedirectToAction("ShelfOverview", "Shelf");
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult RemoveFromShelf(int bookId, int shelfId)
        {
            RemoveBookFromShelf(bookId, shelfId);
            return RedirectToAction("ShelfDetails", "Shelf", new { id = shelfId });
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult RemoveFromCurrentBookShelf(int bookId, int shelfId)
        {
            RemoveBookFromShelf(bookId, shelfId);
            return RedirectToAction("CurrentlyReadingOverview", "Shelf");
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult UpdateCurrentBookProgress(CurrentBookViewModel book, int progress)
        {
            try
            {
                CurrentBookDTO bookDTO = _mapper.Map<CurrentBookDTO>(book);
                _shelfService.UpdateBookProgress(bookDTO, progress);
            }
            catch (InvalidProgressException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("CurrentlyReadingOverview", "Shelf");
        }


        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult MoveToShelf(int bookId, int shelfId)
        {
            try
            {
                _shelfService.AddBookToShelf(bookId, shelfId);
                TempData["Success"] = "Book successfully added to shelf!";
            }
            catch(BookIsAlreadyOnShelfException ex)
            {
                TempData["Warning"] = ex.Message;
            }
            return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult RemoveShelf(int id)
        {
            try
            {
                _shelfService.RemoveShelf(id);
                TempData["Success"] = "Shelf was removed successfully!";
            }
            catch(NullReferenceException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("ShelfOverview", "Shelf");
        }

        private void RemoveBookFromShelf(int bookId, int shelfId)
        {
            try
            {
                RegularShelfDTO? shelf = _shelfService.GetShelfById(shelfId);
                _shelfService.RemoveBookFromShelf(bookId, shelf);
                TempData["Success"] = "Book was removed successfully!";
            }
            catch(NullReferenceException ex)
            {
                TempData["ShelfBookError"] = ex.Message;
            }
        }
    }
}
