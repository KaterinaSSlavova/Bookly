using Models.Entities;
using Business_logic.InterfacesServices;
using Bookly.Business_logic.InterfacesServices;
using Models.Enums;
using ViewModels.Model;
using AutoMapper;
using Bookly.Business_logic.Services;
using Newtonsoft.Json;

namespace Business_logic.Services
{
    public class RandomServices: IRandomServices
    {
        private readonly IShelfServices _ishelfService;
        private readonly IBookServices _ibookService;   
        private readonly IRatingServices _ratingServices;
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;   
        public RandomServices(IShelfServices ishelfService, IBookServices ibookService, IRatingServices ratingServices, IMapper mapper, IUserServices userServices)
        { 
            _ishelfService = ishelfService;
            _ibookService = ibookService;
            _ratingServices = ratingServices;
            _mapper = mapper;
            _userServices = userServices;
        }

        private List<BookViewModel>? GetHaveReadShelf()
        {
            User user = GetUser();
            foreach(ShelfViewModel shelf in _ishelfService.GetUserShelves(user.Id))
            {
                if(shelf.Name == "Have Read")
                {
                   return _ishelfService.GetBooksFromShelf(shelf.Id);
                }
            }
            return null;
        }

        public List<BookViewModel> GetUnreadBooks()
        {
            List<BookViewModel>? readBooks = GetHaveReadShelf();
            List<BookViewModel> allBooks = _ibookService.LoadBooks();
            if(readBooks != null)
            {
                List<BookViewModel> unreadBooks = new List<BookViewModel>();
                List<int> readBooksId = readBooks.Select(x => x.Id).ToList();   
                foreach(BookViewModel book in allBooks)
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

        public BookViewModel RandomResult() 
        {
            List<BookViewModel> books = GetUnreadBooks();
            Random random = new Random();
            int index = random.Next(books.Count);
            BookViewModel randomBook = books[index];
            return randomBook;
        }

        public List<BookViewModel> FilterBooks(Genre genre, Ratings rating)
        {
            List<Book> filteredBooks = new List<Book>();
            List<BookViewModel> unreadBooks = GetUnreadBooks();
            foreach (BookViewModel book in unreadBooks)
            {
                filteredBooks.Add(_mapper.Map<Book>(book));
            }
            filteredBooks = filteredBooks.Where(b => b.Genre == genre).ToList();
            filteredBooks = filteredBooks.Where(b => _ratingServices.GetMostPopularRating(b.Id) == rating).ToList();
            List<BookViewModel> filteredModels = new List<BookViewModel>();
            foreach (Book book in filteredBooks)
            {
                filteredModels.Add(_mapper.Map<BookViewModel>(book));
            }
            return filteredModels;   
        }

        public DateWithABookViewModel DateWithBook(string filteredJson)
        {
            List<BookViewModel> filteredBooks = filteredJson != null ? JsonConvert.DeserializeObject<List<BookViewModel>>(filteredJson) : new List<BookViewModel>();
            return new DateWithABookViewModel()
            {
                filteredBooksModel = filteredBooks,
                Genres = _ibookService.GetAllGenres(),
                Ratings = _ratingServices.GetAllRatings()
            };
        }

        private User GetUser()
        {
            return _userServices.LoadUser();
        }
    }
}
