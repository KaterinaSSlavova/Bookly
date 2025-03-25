using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;

namespace Bookly.Bookly.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookServices _ibookService;
        private readonly IShelfServices _ishelfService;
        private readonly IUserServices _iuserService;

        public BookController(IBookServices ibookService, IShelfServices ishelfService, IUserServices iuserService)
        {
            this._ibookService = ibookService;
            this._ishelfService = ishelfService;
            this._iuserService = iuserService;
        }

        [HttpGet]
        public IActionResult BookDetails(int id)   
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            Book? book = _ibookService.GetBookById(id); 
            User user = _iuserService.LoadUser(ViewBag.Username);
            ViewBag.Shelves = _ishelfService.GetUserShelves(user.Id);
            return View(book);
        }

        [HttpPost]
        public IActionResult GoBack()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult MoveToShelf(int bookId, int shelfId)
        {
            string? username = HttpContext.Session.GetString("Username");
            User? user = _iuserService.LoadUser(username);
            if (_ishelfService.AddBookToShelf(bookId, shelfId, user.Id))
            {
                TempData["Message"] = "Book added to shelf!";
            }
            return RedirectToAction("BookDetails", "Book",new { id=bookId });
        }

        [HttpPost]
        public IActionResult GetBookDetails(int id)
        {
            return RedirectToAction("BookDetails", "Book",new { id = id });
        }

        [HttpGet]
        public IActionResult AddBookPage()
        {
            ViewBag.Username= HttpContext.Session.GetString("Username");
            return View();
        }

        [HttpPost]
        public IActionResult GoToAddBook()
        {
            return RedirectToAction("AddBookPage", "Book");
        }

        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            _ibookService.AddBook(book);
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
