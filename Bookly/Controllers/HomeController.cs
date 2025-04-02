using System.Diagnostics;
using Azure.Identity;
using Bookly.Business_logic.InterfacesServices;
using Models.Entities;
using Bookly.Business_logic.Services;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Model;

namespace Bookly.Bookly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookServices _ibookServices;
        private readonly IUserServices _iuserService;

        public HomeController(ILogger<HomeController> logger, IBookServices ibookServices, IUserServices iuserService)
        {
            _logger = logger;
            this._ibookServices = ibookServices;
            this._iuserService = iuserService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Username=HttpContext.Session.GetString("Username");
            List<BookViewModel> books = _ibookServices.LoadBooks();
            return View(books);
        }

        [HttpPost]
        public IActionResult ViewBook(int id)
        {
            return RedirectToAction("BookDetails","Book", new { id = id });
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
