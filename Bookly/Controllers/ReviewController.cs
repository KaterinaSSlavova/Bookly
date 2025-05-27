using Bookly.Filters;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewServices _reviewServices;
        public ReviewController(IReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult CreateReview(string description, int bookId)
		{
			_reviewServices.AddReview(description, bookId);
            TempData["Success"] = "Review was successfully created!";

			return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
        }

        [FilterLoggedUsers]
        [HttpPost]
        public IActionResult RemoveReview(int reviewId, int bookId)
        {
			_reviewServices.RemoveReview(reviewId);
			TempData["Success"] = "Review was successfully removed!";
			return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
		}
    }
}
