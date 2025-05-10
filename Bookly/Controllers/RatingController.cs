using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    public class RatingController : Controller
    {
        private readonly ILogger<RatingController> _logger;
        private readonly IRatingServices _ratingService;

        public RatingController(IRatingServices ratingService, ILogger<RatingController> logger)
        {
            _ratingService = ratingService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult RateABook(int bookId, int ratingId)
        {
            try
            {
                if (_ratingService.RateBook(bookId, ratingId))
                {
                    TempData["Rating"] = "Rating successful!";
                }
                return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to rate a book: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("Index", "Book");
            }
        }
    }
}