using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewServices _reviewServices;
        private readonly IBookServices _bookServices;
        public ReviewController(IReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }

        [HttpPost]
        public IActionResult CreateReview(string description, int bookId)
        {
            if(_reviewServices.AddReview(description, bookId))
            {
                TempData["Review"] = "Review was successfully created!";
            }
            return RedirectToAction("BookDetails","Book", new { bookId = bookId });
        }
    }
}
