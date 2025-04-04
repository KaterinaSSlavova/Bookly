using Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Newtonsoft.Json;
using ViewModels.Model;

namespace Bookly.Controllers
{
    public class RandomController : Controller
    {
        private readonly IRandomServices _iRandomServices;
        public RandomController(IRandomServices iRandomServices)
        {
            this._iRandomServices = iRandomServices;
        }

        [HttpGet]
        public IActionResult SpinTheWheel()
        {
            List<BookViewModel> unreadBooks = _iRandomServices.GetUnreadBooks();
            return View(unreadBooks);
        }

        [HttpPost]
        public IActionResult Spin()
        {
            TempData["Book"] = _iRandomServices.RandomResult().Title;
            return RedirectToAction("SpinTheWheel", "Random");
        }

        [HttpGet]
        public IActionResult DateWithABook()
        {
            var filteredBooksJson = TempData["Filtered"] as string;
            DateWithABookViewModel model = _iRandomServices.DateWithBook(filteredBooksJson);
            return View(model);
        }

        [HttpPost]
        public IActionResult FilterBooks(Ratings ratings, Genre genre)
        {
            List<BookViewModel> filteredBooks = _iRandomServices.FilterBooks(genre, ratings);
            TempData["Filtered"] = JsonConvert.SerializeObject(filteredBooks);  
            return RedirectToAction("DateWithABook", "Random");
        }
    }
}
