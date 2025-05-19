using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.ViewModels;
using Business_logic.DTOs;
using Business_logic.Exceptions;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index()
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
                AddBookModel model = new AddBookModel();
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
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all fields!";
                return RedirectToAction("AddBookPage", "Book");
            }

            try
            {
                BookDTO book = _mapper.Map<BookDTO>(bookModel);
                _bookService.AddBook(book);
                TempData["Success"] = "Book added successfully!";
                return RedirectToAction("Index", "Book");

            }
            catch (ServiceValidationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (InvalidBookPagesException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (DuplicateISBNException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("AddBookPage", "Book");
        }

        [HttpGet]
        public IActionResult UpdateBook(int bookId)
        {
            BookDetailsDTO bookDTO = _bookDetailsService.CreateDetailsDTO(bookId);
            AddBookModel model = new AddBookModel(bookId, bookDTO.Book.Picture, bookDTO.Book.Title, bookDTO.Book.Author, bookDTO.Book.Description, bookDTO.Book.ISBN, bookDTO.Book.Genre.ToString(), bookDTO.Book.Pages);
            return View(model);
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
                TempData["Success"] = "Book updated successfully!";
                return RedirectToAction("BookDetails", "Book", new { bookId = bookModel.Id });
            }
            catch (ServiceValidationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (InvalidBookPagesException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (DuplicateISBNException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("UpdateBook", "Book", new { bookId = bookModel.Id });
        }

        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            try
            {
                _bookService.RemoveBook(id);
                TempData["BookCatalogSuccess"] = "Book was removed successfully!";
            }
            catch (ServiceValidationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index", "Book");
        }
    }
}
