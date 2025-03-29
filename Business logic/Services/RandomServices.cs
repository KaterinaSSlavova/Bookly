using Bookly.Business_logic.Services;
using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Bookly.Business_logic.InterfacesServices;

namespace Business_logic.Services
{
    public class RandomServices: IRandomServices
    {
        private readonly IShelfServices _ishelfService;
        private readonly IBookServices _ibookService;   
        public RandomServices(IShelfServices ishelfService, IBookServices ibookService)
        { 
            _ishelfService = ishelfService;
            _ibookService = ibookService;
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

        public Book RandomResult(int userId)
        {
            List<Book> books = GetUnreadBooks(userId);
            Random random = new Random();
            int index = random.Next(books.Count);
            Book randomBook = books[index];
            return randomBook;
        }
    }
}
