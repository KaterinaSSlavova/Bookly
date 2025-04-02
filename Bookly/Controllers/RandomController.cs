using Bookly.Business_logic.InterfacesServices;
using Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.Enums;
using Newtonsoft.Json;
using ViewModels.Model;

namespace Bookly.Controllers
{
    public class RandomController : Controller
    {
        private readonly IRandomServices _iRandomServices;
        private readonly IUserServices _iuserService;
        private readonly IBookServices bookServices;
        private readonly IRatingServices ratingServices;
        public RandomController(IRandomServices iRandomServices, IUserServices iuserServices, IRatingServices ratingServices, IBookServices bookServices)
        {
            this._iRandomServices = iRandomServices;
            this._iuserService = iuserServices;
            this.ratingServices = ratingServices;   
            this.bookServices = bookServices;
        }

        [HttpGet]
        public IActionResult SpinTheWheel()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            User user = _iuserService.LoadUser(ViewBag.Username);
            List<BookViewModel> unreadBooks = _iRandomServices.GetUnreadBooks(user.Id);
            return View(unreadBooks);
        }

        [HttpPost]
        public IActionResult Spin()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            User user = _iuserService.LoadUser(ViewBag.Username);
            TempData["Book"] = _iRandomServices.RandomResult(user.Id).Title;
            return RedirectToAction("SpinTheWheel", "Random");
        }

        [HttpGet]
        public IActionResult DateWithABook()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            User user = _iuserService.LoadUser(ViewBag.Username);
            //ViewBag.Ratings = ratingServices.GetAllRatings();
            //ViewBag.Genres = bookServices.GetAllGenres();
            var filteredBooksJson = TempData["Filtered"] as string;
            List<BookViewModel> filteredBooks = filteredBooksJson != null ? JsonConvert.DeserializeObject<List<BookViewModel>>(filteredBooksJson) : new List<BookViewModel>();
            DateWithABookViewModel model = new DateWithABookViewModel()
            {
                filteredBooksModel = filteredBooks,
                Genres = bookServices.GetAllGenres(),
                Ratings = ratingServices.GetAllRatings()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult FilterBooks(Ratings ratings, Genre genre)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            User user = _iuserService.LoadUser(ViewBag.Username);
            List<BookViewModel> filteredBooks = _iRandomServices.FilterBooks(user.Id, genre, ratings);
            TempData["Filtered"] = JsonConvert.SerializeObject(filteredBooks);  
            return RedirectToAction("DateWithABook", "Random");
        }
    }
}
