using Bookly.Business_logic.InterfacesServices;
using Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Bookly.Controllers
{
    public class RandomController : Controller
    {
        private readonly IRandomServices _iRandomServices;
        private readonly IUserServices _iuserService;
        public RandomController(IRandomServices iRandomServices, IUserServices iuserServices)
        {
            this._iRandomServices = iRandomServices;
            this._iuserService = iuserServices; 
        }

        [HttpGet]
        public IActionResult SpinTheWheel()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            User user = _iuserService.LoadUser(ViewBag.Username);
            List<Book> unreadBooks = _iRandomServices.GetUnreadBooks(user.Id);
            return View(unreadBooks);
        }

        [HttpPost]
        public IActionResult Spin()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            User user = _iuserService.LoadUser(ViewBag.Username);
            TempData["Book"] = _iRandomServices.RandomResult(user.Id).Title;
            return RedirectToAction("SpinTheWheel", "Random");
        }

        [HttpGet]
        public IActionResult DateWithABook()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            User user = _iuserService.LoadUser(ViewBag.Username);
            return View();
        }

        [HttpPost]
        public IActionResult FilterBooks()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            User user = _iuserService.LoadUser(ViewBag.Username);
            return RedirectToAction("DateWithABook", "Random");
        }
    }
}
