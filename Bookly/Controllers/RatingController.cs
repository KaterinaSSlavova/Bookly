using Bookly.Filters;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    [FilterLoggedUsers]
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
			_ratingService.RateBook(bookId, ratingId);
			TempData["Success"] = "Rating successful!";
			return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
		}
    }
}