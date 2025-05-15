using Bookly.Business_logic.InterfacesServices;
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

        [HttpPost]
        public IActionResult CreateReview(string description, int bookId)
		{
			_reviewServices.AddReview(description, bookId);
            TempData["Review"] = "Review was successfully created!";

			return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
        }

        [HttpPost]
        public IActionResult RemoveReview(int reviewId, int bookId)
        {
			_reviewServices.RemoveReview(reviewId);
			TempData["Review"] = "Review was successfully removed!";
			return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
		}
    }
}
