using Models.Entities;
using Business_logic.InterfacesServices;
using Bookly.Business_logic.InterfacesServices;
using Models.Enums;
using ViewModels.Model;
using AutoMapper;

namespace Business_logic.Services
{
    public class RandomServices: IRandomServices
    {
        private readonly IShelfServices _ishelfService;
        private readonly IBookServices _ibookService;   
        private readonly IRatingServices ratingServices;
        private readonly IMapper _mapper;
        public RandomServices(IShelfServices ishelfService, IBookServices ibookService, IRatingServices ratingServices, IMapper mapper)
        { 
            _ishelfService = ishelfService;
            _ibookService = ibookService;
            this.ratingServices = ratingServices;
            _mapper = mapper;
        }

        private List<BookViewModel>? GetHaveReadShelf(int userId)
        {
            foreach(ShelfViewModel shelf in _ishelfService.GetUserShelves(userId))
            {
                if(shelf.Name == "Have Read")
                {
                   return _ishelfService.GetBooksFromShelf(shelf.Id);
                }
            }
            return null;
        }

        public List<BookViewModel> GetUnreadBooks(int userId)
        {
            List<BookViewModel>? readBooks = GetHaveReadShelf(userId);
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

        /// <summary>
        /// Generates a random number and takes the book, which id is equal to the number.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>randomly chosen book</returns>
        public BookViewModel RandomResult(int userId) 
        {
            List<BookViewModel> books = GetUnreadBooks(userId);
            Random random = new Random();
            int index = random.Next(books.Count);
            BookViewModel randomBook = books[index];
            return randomBook;
        }

        /// <summary>
        /// Filters all books the user has not read, by genre and stars for Random date with a book.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="genre"></param>
        /// <param name="stars"></param>
        /// <returns>Filtered list of a books</returns>
        public List<BookViewModel> FilterBooks(int userId, Genre genre, Ratings rating)
        {
            List<Book> filteredBooks = new List<Book>();
            List<BookViewModel> unreadBooks = GetUnreadBooks(userId);
            foreach (BookViewModel book in unreadBooks)
            {
                filteredBooks.Add(_mapper.Map<Book>(book));
            }
            filteredBooks = filteredBooks.Where(b => b.Genre == genre).ToList();
            filteredBooks = filteredBooks.Where(b => ratingServices.GetMostPopularRating(b.Id) == rating).ToList();
            List<BookViewModel> filteredModels = new List<BookViewModel>();
            foreach (Book book in filteredBooks)
            {
                filteredModels.Add(_mapper.Map<BookViewModel>(book));
            }
            return filteredModels;   
        }
    }
}
