using Models.Entities;
using Business_logic.InterfacesServices;
using Bookly.Business_logic.InterfacesServices;
using Models.Enums;

namespace Business_logic.Services
{
    public class RandomServices: IRandomServices
    {
        private readonly IShelfServices _ishelfService;
        private readonly IBookServices _ibookService;   
        private readonly IRatingServices ratingServices;
        public RandomServices(IShelfServices ishelfService, IBookServices ibookService, IRatingServices ratingServices)
        { 
            _ishelfService = ishelfService;
            _ibookService = ibookService;
            this.ratingServices = ratingServices;
        }

        private List<Book>? GetHaveReadShelf(int userId)
        {
            foreach(Shelf shelf in _ishelfService.GetUserShelves(userId))
            {
                if(shelf.Name == "Have Read")
                {
                   return _ishelfService.GetBooksFromShelf(shelf.Id);
                }
            }
            return null;
        }

        public List<Book> GetUnreadBooks(int userId)
        {
            List<Book>? readBooks = GetHaveReadShelf(userId);
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

        /// <summary>
        /// Generates a random number and takes the book, which id is equal to the number.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>randomly chosen book</returns>
        public Book RandomResult(int userId) 
        {
            List<Book> books = GetUnreadBooks(userId);
            Random random = new Random();
            int index = random.Next(books.Count);
            Book randomBook = books[index];
            return randomBook;
        }

        /// <summary>
        /// Filters all books the user has not read, by genre and stars for Random date with a book.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="genre"></param>
        /// <param name="stars"></param>
        /// <returns>Filtered list of a books</returns>
        public List<Book> FilterBooks(int userId, Genre genre, Ratings rating)
        {
            List<Book> filteredBooks = GetUnreadBooks(userId);
            filteredBooks = filteredBooks.Where(b => b.Genre == genre).ToList();
            filteredBooks = filteredBooks.Where(b => ratingServices.GetMostPopularRating(b.Id) == rating).ToList();
            return filteredBooks;   
        }
    }
}
