using System.Diagnostics;
using Bookly.Business_logic.InterfacesServices;
using Models.Entities;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Model;

namespace Bookly.Bookly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookServices _ibookServices;

        public HomeController(ILogger<HomeController> logger, IBookServices ibookServices)
        {
            _logger = logger;
            _ibookServices = ibookServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<BookViewModel> books = _ibookServices.LoadBooks();
            return View(books);
        }

        [HttpPost]
        public IActionResult ViewBook(int id)
        {
            return RedirectToAction("BookDetails","Book", new { bookId = id });
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
