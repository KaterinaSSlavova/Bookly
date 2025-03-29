using Bookly.Business_logic.InterfacesServices;
using Bookly.Business_logic.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Bookly.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ReviewServices _services;
        private readonly IUserServices _userServices;
        private readonly IBookServices _bookServices;
        public ReviewController(ReviewServices services, IUserServices userServices, IBookServices bookServices)
        {
            _services = services;
            _userServices = userServices;
            _bookServices = bookServices;
        }

        [HttpPost]
        public IActionResult CreateReview(string description, int bookId)
        {
            User user = _userServices.LoadUser(HttpContext.Session.GetString("Username"));
            Book book = _bookServices.GetBookById(bookId);
            Review review = new Review(description, user, book);
            if(_services.AddReview(review))
            {
                TempData["Review"] = "Review was successfully created!";
            }
            return RedirectToAction("BookDetails","Book", new { id = bookId });
        }
    }
}
