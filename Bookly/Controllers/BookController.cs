using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.ViewModels;
using Business_logic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Bookly.Bookly.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookServices _bookService;
        private readonly IBookDetailsService _bookDetailsService;
        private readonly IMapper _mapper;

        public BookController(ILogger<BookController> logger, IBookServices bookService, IMapper mapper, IBookDetailsService bookDetailsService)
        {
            _bookService = bookService;
            _mapper = mapper;
            _bookDetailsService = bookDetailsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                List<BookDTO> books = _bookService.LoadBooks();
                List<BookViewModel> booksModel = _mapper.Map<List<BookViewModel>>(books);
                return View(booksModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to load home page: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An unexpected error occurred! Please try again later!";
                return View();
            }
        }

        [HttpPost]
        public IActionResult ViewBook(int id)
        {
            return RedirectToAction("BookDetails", "Book", new { bookId = id });
        }

        [HttpGet]
        public IActionResult BookDetails(int bookId)
        {
            try
            {
                BookDetailsDTO bookDTO = _bookDetailsService.CreateDetailsDTO(bookId);
                BookDetailsViewModel model = _mapper.Map<BookDetailsViewModel>(bookDTO);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to load book details: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("Index", "Book");
            }
        }

        [HttpPost]
        public IActionResult GoBack()
        {
            return RedirectToAction("Index", "Book");
        }

        [HttpPost]
        public IActionResult GetBookDetails(int id)
        {
            return RedirectToAction("BookDetails", "Book", new { bookid = id });
        }

        [HttpGet]
        public IActionResult AddBookPage()
        {
            try
            {
                AddBookModel model = new AddBookModel();
                return View(model);
            }
            catch (ArgumentException ex)
            {
                TempData["BookError"] = ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to load add book page: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An unexpected error occurred! Please try again later!";
                return View();
            }
        }

        [HttpPost]
        public IActionResult GoToAddBook()
        {
            return RedirectToAction("AddBookPage", "Book");
        }

        [HttpPost]
        public IActionResult AddBook(AddBookModel bookModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["BookError"] = "Please fill all fields!";
                return RedirectToAction("AddBookPage", "Book");
            }

            try
            {
                BookDTO book = _mapper.Map<BookDTO>(bookModel);
                _bookService.AddBook(book);
                TempData["BookSuccess"] = "Book added successfully!";
                return RedirectToAction("Index", "Book");

            }
            catch (ArgumentException ex)
            {
                TempData["BookError"] = ex.Message;
                return RedirectToAction("AddBookPage", "Book");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An sql error occurred while trying to add a book: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An unexpected error occurred while trying to save this book! Please try again later!";
                return RedirectToAction("AddBookPage", "Book");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while trying to add a book: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("AddBookPage", "Book");
            }
        }

        [HttpGet]
        public IActionResult UpdateBook(int bookId)
        {
            try
            {
                BookDetailsDTO bookDTO = _bookDetailsService.CreateDetailsDTO(bookId);
                AddBookModel model = new AddBookModel(bookId, bookDTO.Book.Picture, bookDTO.Book.Title, bookDTO.Book.Author, bookDTO.Book.Description, bookDTO.Book.ISBN, bookDTO.Book.Genre.ToString(), bookDTO.Book.Pages);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to load update book page: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("Index", "Book");
            }
        }

        [HttpPost]
        public IActionResult GoToUpdateBook(int Id)
        {
            return RedirectToAction("UpdateBook", "Book", new { bookId = Id });
        }

        [HttpPost]
        public IActionResult SaveBookChanges(AddBookModel bookModel)
        {
            try
            {
                BookDTO book = _mapper.Map<BookDTO>(bookModel);
                _bookService.UpdateBook(book);
                TempData["Message"] = "Book updated successfully!";
                return RedirectToAction("BookDetails", "Book", new { bookId = bookModel.Id });
            }
            catch (ArgumentException ex)
            {
                TempData["BookError"] = ex.Message;
                return RedirectToAction("UpdateBook", "Book", new { bookId = bookModel.Id });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An sql error occurred while trying to update a book: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An error occurred while trying to update this book! Please try again later!";
                return RedirectToAction("AddBookPage", "Book");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to update a book: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("", "Book");
            }
        }

        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            try
            {
                _bookService.RemoveBook(id);
                TempData["BookCatalogSuccess"] = "Book was removed successfully!";
                return RedirectToAction("Index", "Book");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "An error occurred while trying to remove a book: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "Book was not found! Please try again later!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred: {ErrorMessage}", ex.Message);
                TempData["BookError"] = "An unexpected error occurred! Please try again later!";
            }
            return RedirectToAction("Index", "Book");
        }
    }
}
