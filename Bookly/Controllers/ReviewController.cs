using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewServices _reviewServices;
        public ReviewController(IReviewServices reviewServices, ILogger<ReviewController> logger)
        {
            _reviewServices = reviewServices;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreateReview(string description, int bookId)
        {
            try
            {
                if (_reviewServices.AddReview(description, bookId))
                {
                    TempData["Review"] = "Review was successfully created!";
                }
                return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to create a review: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("Index", "Book");
            }
        }

        [HttpPost]
        public IActionResult RemoveReview(int reviewId, int bookId)
        {
            try
            {
                if (_reviewServices.RemoveReview(reviewId))
                {
                    TempData["Review"] = "Review was successfully removed!";
                }
                return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to remove a review: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("Index", "Book");
            }
        }
    }
}
