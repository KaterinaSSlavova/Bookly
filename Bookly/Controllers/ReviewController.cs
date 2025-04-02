using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using ViewModels.Model;

namespace Bookly.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewServices _services;
        private readonly IUserServices _userServices;
        private readonly IBookServices _bookServices;
        public ReviewController(IReviewServices services, IUserServices userServices, IBookServices bookServices)
        {
            _services = services;
            _userServices = userServices;
            _bookServices = bookServices;
        }

        [HttpPost]
        public IActionResult CreateReview(string description, int bookId)
        {
            User user = _userServices.LoadUser(HttpContext.Session.GetString("Username"));
            BookViewModel book = _bookServices.GetBookById(bookId);
            if(_services.AddReview(description, user, book))
            {
                TempData["Review"] = "Review was successfully created!";
            }
            return RedirectToAction("BookDetails","Book", new { id = bookId });
        }
    }
}
