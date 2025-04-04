using Microsoft.AspNetCore.Mvc;
using Bookly.Business_logic.InterfacesServices;
using ViewModels.Model;

namespace Bookly.Bookly.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookServices _ibookService;
        private readonly IShelfServices _shelfService;

        public BookController(IBookServices ibookService, IShelfServices shelfService)
        {
            _ibookService = ibookService;
            _shelfService = shelfService;
        }

        [HttpGet]
        public IActionResult BookDetails(int bookId)   
        {
            BookDetailsViewModel model = _ibookService.GetBookDetails(bookId);
            return View(model);
        }

        [HttpPost]
        public IActionResult GoBack()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult MoveToShelf(int bookId, int shelfId)
        {
            if (_shelfService.AddBookToShelf(bookId, shelfId))
            {
                TempData["Message"] = "Book added to shelf!";
            }
            return RedirectToAction("BookDetails", "Book",new { bookId=bookId });
        }

        [HttpPost]
        public IActionResult GetBookDetails(int id)
        {
            return RedirectToAction("BookDetails", "Book",new { bookid = id });
        }

        [HttpGet]
        public IActionResult AddBookPage()
        {
            BookViewModel bookModel = new BookViewModel()
            {
                Genres = _ibookService.GetAllGenres(),  
            };  
            return View(bookModel);
        }

        [HttpPost]
        public IActionResult GoToAddBook()
        {
            return RedirectToAction("AddBookPage", "Book");
        }

        [HttpPost]
        public IActionResult AddBook(BookViewModel bookModel)
        {
            _ibookService.AddBook(bookModel);
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            _ibookService.RemoveBook(id);
            return RedirectToAction("Index", "Home");
        }
    }
}
