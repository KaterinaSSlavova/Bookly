using System.Diagnostics;
using Azure.Identity;
using Bookly.Models;
using Bookly.Repository;
using Bookly.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookServices _bookService;
        private readonly UserServices _userService;

        public HomeController(ILogger<HomeController> logger, BookServices bookService, UserServices userService)
        {
            _logger = logger;
            this._bookService = bookService;
            this._userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Username=HttpContext.Session.GetString("Username");
            List<Book> books = _bookService.LoadBooks();
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
