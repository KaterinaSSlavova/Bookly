using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;
using ViewModels.Model;
using Models.Enums;

namespace Bookly.Bookly.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookServices _ibookService;
        private readonly IShelfServices _ishelfService;
        private readonly IUserServices _iuserService;
        private readonly IReviewServices _reviewService;
        private readonly IRatingServices _ratingService;

        public BookController(IBookServices ibookService, IShelfServices ishelfService, IUserServices iuserService, IReviewServices reviewService, IRatingServices ratingService)
        {
            this._ibookService = ibookService;
            this._ishelfService = ishelfService;
            this._iuserService = iuserService;
            _reviewService = reviewService;
            _ratingService = ratingService;
        }

        [HttpGet]
        public IActionResult BookDetails(int id)   
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            BookViewModel? book = _ibookService.GetBookById(id); 
            User user = _iuserService.LoadUser(ViewBag.Username);
            //ViewBag.Shelves = _ishelfService.GetUserShelves(user.Id);
            //ViewBag.Reviews = _reviewService.GetBookReviews(id);
            Ratings? rating = _ratingService.GetUserRatingForBook(user.Id, id);
            List<ShelfViewModel> shelfViewModels = _ishelfService.GetUserShelves(user.Id); 
            List<ReviewViewModel> reviewViewModels = _reviewService.GetBookReviews(id);
            BookDetailsViewModel model = new BookDetailsViewModel(book, shelfViewModels,reviewViewModels, rating.ToString());
            return View(model);
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
