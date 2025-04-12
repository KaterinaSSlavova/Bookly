using Models.Entities;
using Business_logic.InterfacesServices;
using Bookly.Business_logic.InterfacesServices;
using Models.Enums;
using AutoMapper;
using Newtonsoft.Json;
using Business_logic.DTOs;

namespace Business_logic.Services
{
    public class RandomServices: IRandomServices
    {
        private readonly IShelfServices _ishelfService;
        private readonly IBookServices _ibookService;   
        private readonly IRatingServices _ratingServices;
        private readonly IUserServices _userServices;   
        public RandomServices(IShelfServices ishelfService, IBookServices ibookService, IRatingServices ratingServices, IUserServices userServices)
        { 
            _ishelfService = ishelfService;
            _ibookService = ibookService;
            _ratingServices = ratingServices;
            _userServices = userServices;
        }

        private List<Book>? GetHaveReadShelf()
        {
            User user = GetUser();
            foreach(Shelf shelf in _ishelfService.GetUserShelves())
            {
                if(shelf.Name == "Have Read")
                {
                   return _ishelfService.GetBooksFromShelf(shelf.Id);
                }
            }
            return null;
        }

        public List<Book> GetUnreadBooks()
        {
            List<Book>? readBooks = GetHaveReadShelf();
            List<Book> allBooks = _ibookService.LoadBooks();
            if(readBooks != null)
            {
                List<Book> unreadBooks = new List<Book>();
                List<int> readBooksId = readBooks.Select(x => x.Id).ToList();   
                foreach(Book book in allBooks)
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

        public Book RandomResult() 
        {
            List<Book> books = GetUnreadBooks();
            Random random = new Random();
            int index = random.Next(books.Count);
            Book randomBook = books[index];
            return randomBook;
        }

        public List<Book> FilterBooks(Genre genre, Ratings rating)
        {
            List<Book> filteredBooks = GetUnreadBooks();
            filteredBooks = filteredBooks.Where(b => b.Genre == genre).ToList();
            filteredBooks = filteredBooks.Where(b => _ratingServices.GetMostPopularRating(b.Id) == rating).ToList();
            return filteredBooks;   
        }

        public DateWithABookDTO CreateDateDTO(string filteredJson) 
        {
            List<Book> filteredBooks = filteredJson != null ? JsonConvert.DeserializeObject<List<Book>>(filteredJson) : new List<Book>();
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

        private User GetUser()
        {
            return _userServices.LoadUser();
        }
    }
}
