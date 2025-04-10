using System.Diagnostics;
using Bookly.Business_logic.InterfacesServices;
using Models.Entities;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Model;
using AutoMapper;

namespace Bookly.Bookly.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookServices _bookServices;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IBookServices bookServices, IMapper mapper)
        {
            _logger = logger;
            _bookServices = bookServices;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Book> books = _bookServices.LoadBooks();
            List<BookViewModel> booksModel = _mapper.Map<List<BookViewModel>>(books);  
            return View(booksModel);
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
