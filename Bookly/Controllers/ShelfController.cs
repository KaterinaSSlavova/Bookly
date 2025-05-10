using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.ViewModels;
using Business_logic.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Bookly.Controllers
{
    public class ShelfController : Controller
    {
        private readonly ILogger<ShelfController> _logger;
        private readonly IShelfServices _shelfService;
        private readonly IMapper _mapper;
        public ShelfController(IShelfServices shelfService, IMapper mapper, ILogger<ShelfController> logger)
        {
            _shelfService = shelfService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult ShelfOverview()
        {
            try
            {
                List<ShelfDTO> myShelves = _shelfService.GetUserShelves();
                List<ShelfViewModel> model = _mapper.Map<List<ShelfViewModel>>(myShelves);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to load user shelves: {ErrorMessage}", ex.Message);
                TempData["ShelfError"] = "An unexpected error occurred! Please try again later!";
                return View();
            }
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
            if (shelfModel.Name == null)
            {
                TempData["ShelfError"] = "Please fill all fields!";
                return RedirectToAction("CreateShelf", "Shelf");
            }

            try
            {
                ShelfDTO shelf = _mapper.Map<ShelfDTO>(shelfModel);
                if (!_shelfService.CreateShelf(shelf))
                {
                    TempData["ShelfError"] = "Invalid data! Shelf name must be unique!";
                    return RedirectToAction("CreateShelf", "Shelf");
                }
                return RedirectToAction("ShelfOverview", "Shelf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to create new shelf: {ErrorMessage}", ex.Message);
                TempData["ShelfError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("ShelfOverview", "Shelf");
            }
        }


        [HttpGet]
        public IActionResult ShelfDetails(int id)
        {
            try
            {
                ShelfDTO shelf = _shelfService.GetShelfById(id);
                ShelfViewModel shelfModel = _mapper.Map<ShelfViewModel>(shelf);
                return View(shelfModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to load shelf details: {ErrorMessage}", ex.Message);
                TempData["ShelfError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("ShelfOverview", "Shelf");
            }
        }

        [HttpPost]
        public IActionResult ViewShelf(int id)
        {
            return RedirectToAction("ShelfDetails", "Shelf", new { id = id });
        }

        [HttpGet]
        public IActionResult CurrentlyReadingOverview()
        {
            try
            {
                CurrentBookShelfDTO currentBooksShelf = _shelfService.GetCurrentlyReadingShelf();
                CurrentBookShelfViewModel shelfModel = _mapper.Map<CurrentBookShelfViewModel>(currentBooksShelf);
                return View(shelfModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to load currently reading overview page: {ErrorMessage}", ex.Message);
                TempData["ShelfError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("ShelfOverview", "Shelf");
            }
        }

        [HttpPost]
        public IActionResult ViewCurrentlyReadingShelf()
        {
            return RedirectToAction("CurrentlyReadingOverview", "Shelf");
        }

        [HttpPost]
        public IActionResult GoBack()
        {
            return RedirectToAction("ShelfOverview", "Shelf");
        }

        [HttpPost]
        public IActionResult RemoveFromShelf(int bookId, int shelfId)
        {
            RemoveBookFromShelf(bookId, shelfId);
            return RedirectToAction("ShelfDetails", "Shelf", new { id = shelfId });
        }

        [HttpPost]
        public IActionResult RemoveFromCurrentBookShelf(int bookId, int shelfId)
        {
            RemoveBookFromShelf(bookId, shelfId);
            return RedirectToAction("CurrentlyReadingOverview", "Shelf");
        }

        [HttpPost]
        public IActionResult UpdateCurrentBookProgress(CurrentBookViewModel book, int progress)
        {
            try
            {
                CurrentBookDTO bookDTO = _mapper.Map<CurrentBookDTO>(book);
                _shelfService.UpdateBookProgress(bookDTO, progress);
                return RedirectToAction("CurrentlyReadingOverview", "Shelf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to update current book progress: {ErrorMessage}", ex.Message);
                TempData["ShelfError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("ShelfOverview", "Shelf");
            }
        }

        [HttpPost]
        public IActionResult MoveToShelf(int bookId, int shelfId)
        {
            try
            {
                if (_shelfService.AddBookToShelf(bookId, shelfId))
                {
                    TempData["Message"] = "Book added to shelf!";
                }
                else
                {
                    TempData["Warning"] = "This book is already placed on that shelf!";
                }
                return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to add book to shelf: {ErrorMessage}", ex.Message);
                TempData["ShelfError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("ShelfOverview", "Shelf");
            }
        }

        [HttpPost]
        public IActionResult RemoveShelf(int id)
        {
            try
            {
                if (!_shelfService.RemoveShelf(id))
                {
                    TempData["ShelfError"] = "Shelf cannot be removed!";
                }
                else
                {
                    TempData["ShelfSuccess"] = "Shelf was removed successfully!";
                }
                return RedirectToAction("ShelfOverview", "Shelf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to remove a shelf: {ErrorMessage}", ex.Message);
                TempData["ShelfError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("ShelfOverview", "Shelf");
            }
        }

        private void RemoveBookFromShelf(int bookId, int shelfId)
        {
            try
            {
                if (!_shelfService.RemoveBookFromShelf(bookId, shelfId))
                {
                    TempData["RemoveBookError"] = "Book cannot be removed!";
                }
                else
                {
                    TempData["RemoveBookSuccess"] = "Book was removed successfully!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to remove a book from shelf: {ErrorMessage}", ex.Message);
                TempData["ShelfError"] = "An unexpected error occurred! Please try again later!";
            }
        }
    }
}
