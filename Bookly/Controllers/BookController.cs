using AutoMapper;
using Interfaces;
using Bookly.ViewModels;
using Business_logic.DTOs;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Bookly.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Enums;

namespace Bookly.Bookly.Controllers
{
    [FilterLoggedUsers]
    public class BookController : Controller
    {
        private readonly IBookServices _bookService;
        private readonly IBookDetailsService _bookDetailsService;
        private readonly IRatingServices _ratingServices;
        private readonly IMapper _mapper;

        public BookController(IBookServices bookService, IMapper mapper, IBookDetailsService bookDetailsService, IRatingServices ratingServices)
        {
            _bookService = bookService;
            _mapper = mapper;
            _bookDetailsService = bookDetailsService;
            _ratingServices = ratingServices;
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
            model.Ratings = MapRatings(bookDTO.Ratings); 
            model.DeleteModalBooks = new DeleteModalViewModel(model.Book.Id, "id", "Book", model.Book.Title, "Book", "RemoveBook");
            model.DeleteModalReviews = model.Reviews.Select(r => new DeleteModalViewModel(r.Id, "reviewId", model.Book.Id, "bookId", "Review", null, "Review", "RemoveReview")).ToList();
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
            try
            {
                BookDTO book = _mapper.Map<BookDTO>(bookModel);
                _bookService.AddBook(book);
                TempData["Success"] = "Book added successfully!";
                return RedirectToAction("Index", "Book");

            }
            catch (NullReferenceException ex)
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
            catch (NullReferenceException ex)
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
            catch (NullReferenceException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index", "Book");
        }

        private List<SelectListItem> MapRatings(List<Ratings> ratings)
        {
            return ratings.Select(r => new SelectListItem
            {
                Value = ((int)r).ToString(),
                Text = _ratingServices.GetEnumDescription(r)
            }).ToList();
        }
    }
}
