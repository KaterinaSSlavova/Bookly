using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewServices _services;
        private readonly IBookServices _bookServices;
        public ReviewController(IReviewServices services, IBookServices bookServices)
        {
            _services = services;
            _bookServices = bookServices;
        }

        [HttpPost]
        public IActionResult CreateReview(string description, int bookId)
        {
            if(_services.AddReview(description, bookId))
            {
                TempData["Review"] = "Review was successfully created!";
            }
            return RedirectToAction("BookDetails","Book", new { bookId = bookId });
        }
    }
}
