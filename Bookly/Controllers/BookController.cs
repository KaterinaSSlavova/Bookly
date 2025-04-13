using Microsoft.AspNetCore.Mvc;
using Bookly.Business_logic.InterfacesServices;
using ViewModels.Model;
using Business_logic.DTOs;
using AutoMapper;
using Models.Entities;
using Bookly.Business_logic.Services;

namespace Bookly.Bookly.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookServices _bookService;
        private readonly IBookDetailsService _bookDetailsService;
        private readonly IMapper _mapper;

        public BookController(IBookServices bookService, IMapper mapper, IBookDetailsService bookDetailsService)
        {
            _bookService = bookService;
            _mapper = mapper;
            _bookDetailsService = bookDetailsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<BookDTO> books = _bookService.LoadBooks();
            List<BookViewModel> booksModel = _mapper.Map<List<BookViewModel>>(books);
            return View(booksModel);
        }

        [HttpPost]
        public IActionResult ViewBook(int id)
        {
            return RedirectToAction("BookDetails", "Book", new { bookId = id });
        }

        [HttpGet]
        public IActionResult BookDetails(int bookId)   
        {
            BookDetailsDTO bookDTO = _bookDetailsService.CreateDetailsDTO(bookId);
            BookDetailsViewModel model = _mapper.Map<BookDetailsViewModel>(bookDTO);
            return View(model);
        }

        [HttpPost]
        public IActionResult GoBack()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult GetBookDetails(int id)
        {
            return RedirectToAction("BookDetails", "Book",new { bookid = id });
        }

        [HttpGet]
        public IActionResult AddBookPage()
        { 
            var model = new AddBookModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult GoToAddBook()
        {
            return RedirectToAction("AddBookPage", "Book");
        }

        [HttpPost]
        public IActionResult AddBook(AddBookModel bookModel)
        {
            BookDTO book = _mapper.Map<BookDTO>(bookModel);
            if (!_bookService.AddBook(book))
            {
                TempData["BookError"] = "Invalid data! Book must be unique!";
                return RedirectToAction("AddBookPage", "Book");
            }
            else
            {
                TempData["BookSuccess"] = "Book added successfully!";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            if(!_bookService.RemoveBook(id))
            {
                TempData["BookCatalogError"] = "Cannot remove this book!";
                return RedirectToAction("BookDetails", "Book");
            }
            else
            {
                TempData["BookCatalogSuccess"] = "Book was removed successfully!";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
