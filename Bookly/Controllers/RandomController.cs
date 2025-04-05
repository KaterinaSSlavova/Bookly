using Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Newtonsoft.Json;
using ViewModels.Model;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;

namespace Bookly.Controllers
{
    public class RandomController : Controller
    {
        private readonly IRandomServices _iRandomServices;
        private readonly IDateWithBookService _dateService;
        public RandomController(IRandomServices iRandomServices, IDateWithBookService dateService)
        {
            _iRandomServices = iRandomServices;
            _dateService = dateService;
        }

        [HttpGet]
        public IActionResult SpinTheWheel()
        {
            List<Book> unreadBooks = _iRandomServices.GetUnreadBooks();
            return View();
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
            DateWithABookViewModel model = _dateService.GetDateWithABookModel(filteredBooksJson);
            return View(model);
        }

        [HttpPost]
        public IActionResult FilterBooks(Ratings ratings, Genre genre)
        {
            List<Book> filteredBooks = _iRandomServices.FilterBooks(genre, ratings);
            TempData["Filtered"] = JsonConvert.SerializeObject(filteredBooks);  
            return RedirectToAction("DateWithABook", "Random");
        }
    }
}
