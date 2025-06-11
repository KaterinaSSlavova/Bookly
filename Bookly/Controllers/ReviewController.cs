using Bookly.Filters;
using Exceptions;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    [FilterLoggedUsers]
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
            try
            {
                _reviewServices.AddReview(description, bookId);
                TempData["Success"] = "Review was successfully created!";

            }
            catch (NullReferenceException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (InvalidReviewLengthException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (ReviewLengthShortException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
        }

        [HttpPost]
        public IActionResult RemoveReview(int reviewId, int bookId)
        {
			_reviewServices.RemoveReview(reviewId);
			TempData["Success"] = "Review was successfully removed!";
			return RedirectToAction("BookDetails", "Book", new { bookId = bookId });
		}
    }
}
