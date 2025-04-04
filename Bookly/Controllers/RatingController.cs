using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Bookly.Controllers
{
    public class RatingController : Controller
    {
        private readonly IRatingServices _ratingService;

        public RatingController(IRatingServices ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public IActionResult RateABook(int bookId, int ratingId)
        {
            if (_ratingService.RateBook(bookId, ratingId))
            {
                TempData["Rating"] = "Rating successful!";
            }
            return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
        }
    }
}