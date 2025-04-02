using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Bookly.Controllers
{
    public class RatingController : Controller
    {
        private readonly IRatingServices _ratingService;
        private readonly IUserServices _userServices;

        public RatingController(IRatingServices ratingService, IUserServices userServices)
        {
            _ratingService = ratingService;
            _userServices = userServices;
        }

        [HttpPost]
        public IActionResult RateABook(int bookId, int ratingId)
        {
            User user = _userServices.LoadUser(HttpContext.Session.GetString("Username"));
            if (_ratingService.RateBook(user.Id, bookId, ratingId))
            {
                TempData["Rating"] = "Rating successful!";
            }
            return RedirectToAction("BookDetails", "Book", new { id = bookId });
        }
    }
}