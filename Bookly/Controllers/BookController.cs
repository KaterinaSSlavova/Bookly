using Microsoft.AspNetCore.Mvc;
using Bookly.Business_logic.InterfacesServices;
using ViewModels.Model;

namespace Bookly.Bookly.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookServices _bookService;
        private readonly IBookDetailsService _bookDetailsDTOService;
        private readonly IShelfServices _shelfService;

        public BookController(IBookDetailsService bookDTOService, IShelfServices shelfService, IBookServices bookService)
        {
            _bookService = bookService;
            _bookDetailsDTOService = bookDTOService;
            _shelfService = shelfService;
        }

        [HttpGet]
        public IActionResult BookDetails(int bookId)   
        {
            BookDetailsViewModel model = _bookDetailsDTOService.GetBookDetails(bookId);
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
            else
            {
                TempData["Warning"] = "This book is already placed on that shelf!";
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
            var model = new AddBookModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult GoToAddBook()
        {
            return RedirectToAction("AddBookPage", "Book");
        }

        [HttpPost]
        public IActionResult AddBook(AddBookModel bookModel)
        {
            if(!_bookService.AddBook(bookModel))
            {
                TempData["BookError"] = "Invalid data! Book must be unique!";
                return RedirectToAction("AddBookPage", "Book");
            }
            else
            {
                TempData["BookSuccess"] = "Book added successfully!";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            if(!_bookService.RemoveBook(id))
            {
                TempData["BookCatalogError"] = "Cannot remove this book!";
                return RedirectToAction("BookDetails", "Book");
            }
            else
            {
                TempData["BookCatalogSuccess"] = "Book was removed successfully!";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
