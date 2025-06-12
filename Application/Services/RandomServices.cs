using Interfaces;
using Business_logic.DTOs;
using Models.Enums;
using Newtonsoft.Json;
using Exceptions;

namespace Business_logic.Services
{
    public class RandomServices : IRandomServices
    {
        private readonly IShelfServices _shelfService;
        private readonly IBookServices _bookService;
        private readonly IRatingServices _ratingServices;
        private readonly IUserServices _userServices;
        private const string completedShelf = "Have Read";
        public RandomServices(IShelfServices shelfService, IBookServices bookService, IRatingServices ratingServices, IUserServices userServices)
        {
            _shelfService = shelfService;
            _bookService = bookService;
            _ratingServices = ratingServices;
            _userServices = userServices;
        }

        private List<BookDTO>? GetBooksFromHaveReadShelf()
        {
            foreach (RegularShelfDTO shelf in _shelfService.GetUserShelves())
            {
                if(shelf.Shelf.Name == completedShelf) return shelf.BooksOnShelf;
            }
            return null;
        }

        public List<BookDTO> GetUnreadBooks()
        {
            List<BookDTO>? readBooks = GetBooksFromHaveReadShelf();
            List<BookDTO> allBooks = _bookService.LoadBooks();
            if (readBooks != null)
            {
                List<BookDTO> unreadBooks = new List<BookDTO>();
                List<int> readBooksId = readBooks.Select(x => x.Id).ToList();
                foreach (BookDTO book in allBooks)
                {
                    if (!readBooksId.Contains(book.Id))
                    {
                        unreadBooks.Add(book);
                    }
                }
                return unreadBooks;
            }
            return allBooks;
        }

        public BookDTO RandomResult()
        {
            List<BookDTO> books = GetUnreadBooks();
            Random random = new Random();
            int index = random.Next(books.Count);
            BookDTO randomBook = books[index];
            return randomBook;
        }

        public List<BookDTO> FilterBooks(Genre? genre, Ratings? rating)
        {
            if (genre == null || rating == null)
                throw new ArgumentException("Please select both genre and rating!");
            List<BookDTO> filteredBooks = GetUnreadBooks();
            filteredBooks = filteredBooks.Where(b => b.Genre == genre).ToList();
            filteredBooks = filteredBooks.Where(b => _ratingServices.GetMostPopularRating(b.Id) == rating).ToList();
            return filteredBooks;
        }

        public void AddToWishList(BookDTO book)
        {
            RegularShelfDTO shelf = _shelfService.GetUserWishList();
            if (_shelfService.CheckForBook(shelf, book.Id))
            {
                throw new BookIsAlreadyOnShelfException(shelf.Shelf.Name, book.Title);
            }
            _shelfService.AddBookToShelf(book, shelf);
        }

        public DateWithABookDTO CreateDateDTO(string filteredJson)
        {
            List<BookDTO> filteredBooks = filteredJson != null ? JsonConvert.DeserializeObject<List<BookDTO>>(filteredJson) : new List<BookDTO>();
            return new DateWithABookDTO(filteredBooks, GetGenres(), GetRatings());
        }

        private List<Genre> GetGenres()
        {
            return Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();
        }

        private List<Ratings> GetRatings()
        {
            return Enum.GetValues(typeof(Ratings)).Cast<Ratings>().ToList();
        }
    }
}
