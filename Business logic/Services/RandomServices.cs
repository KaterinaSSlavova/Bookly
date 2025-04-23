using Business_logic.InterfacesServices;
using Bookly.Business_logic.InterfacesServices;
using Models.Enums;
using Newtonsoft.Json;
using Business_logic.DTOs;

namespace Business_logic.Services
{
    public class RandomServices: IRandomServices
    {
        private readonly IShelfServices _shelfService;
        private readonly IBookServices _bookService;   
        private readonly IRatingServices _ratingServices;
        private readonly IUserServices _userServices;   
        public RandomServices(IShelfServices shelfService, IBookServices bookService, IRatingServices ratingServices, IUserServices userServices)
        { 
            _shelfService = shelfService;
            _bookService = bookService;
            _ratingServices = ratingServices;
            _userServices = userServices;
        }

        private List<BookDTO>? GetHaveReadShelf()
        {
            foreach(ShelfDTO shelf in _shelfService.GetUserShelves())
            {
                if(shelf.Name == "Have Read")
                {
                   return _shelfService.GetBooksFromShelf(shelf.Id);
                }
            }
            return null;
        }

        public List<BookDTO> GetUnreadBooks()
        {
            List<BookDTO>? readBooks = GetHaveReadShelf();
            List<BookDTO> allBooks = _bookService.LoadBooks();
            if(readBooks != null)
            {
                List<BookDTO> unreadBooks = new List<BookDTO>();
                List<int> readBooksId = readBooks.Select(x => x.Id).ToList();   
                foreach(BookDTO book in allBooks)
                {
                    if(!readBooksId.Contains(book.Id))
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

        public List<BookDTO> FilterBooks(Genre genre, Ratings rating)
        {
            List<BookDTO> filteredBooks = GetUnreadBooks();
            filteredBooks = filteredBooks.Where(b => b.Genre == genre).ToList();
            filteredBooks = filteredBooks.Where(b => _ratingServices.GetMostPopularRating(b.Id) == rating).ToList();
            return filteredBooks;   
        }

        public bool AddToWishList(BookDTO book)
        {
            ShelfDTO shelf = _shelfService.GetUserWishList();
            if (!_shelfService.CheckForBook(shelf.Id, book.Id))
            {
                return _shelfService.AddBookToShelf(book.Id, shelf.Id);
            }
            return false; 
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
