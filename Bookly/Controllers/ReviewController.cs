using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using ViewModels.Model;

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
            BookViewModel book = _bookServices.GetBookById(bookId);
            if(_services.AddReview(description, book))
            {
                TempData["Review"] = "Review was successfully created!";
            }
            return RedirectToAction("BookDetails","Book", new { bookId = bookId });
        }
    }
}
