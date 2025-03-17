using Microsoft.AspNetCore.Mvc;
using Bookly.Models;
using Bookly.Repository;
using Bookly.Services;

namespace Bookly.Controllers
{
    public class BookController : Controller
    {
        private readonly BookServices _bookService;
        private readonly ShelfServices _shelfService;
        private readonly UserServices _userService;

        public BookController(BookServices bookService, ShelfServices shelfService, UserServices userService)
        {
            this._bookService = bookService;
            this._shelfService = shelfService;
            this._userService = userService;
        }

        [HttpGet]
        public IActionResult BookDetails(int id)   
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            Book? book = _bookService.GetBookById(id); 
            User user = _userService.LoadUser(ViewBag.Username);
            ViewBag.Shelves = _shelfService.GetUserShelves(user.Id);
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
            User? user = _userService.LoadUser(username);
            if (_shelfService.AddBookToShelf(bookId, shelfId, user.Id))
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
            _bookService.AddBook(book);
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            _bookService.RemoveBook(id);
            return RedirectToAction("Index", "Home");   
        }
    }
}
