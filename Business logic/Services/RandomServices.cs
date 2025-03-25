using Bookly.Business_logic.Services;
using Bookly.Data.InterfacesRepo;
using Bookly.Data.Models;
using Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Business_logic.Services
{
    public class RandomServices: IRandomServices
    {
        private readonly IBookRepository _ibookRepo;
        private readonly IShelfRepository _ishelfRepo;
        public RandomServices(IBookRepository ibookRepo, IShelfRepository ishelfREpo)
        {
            this._ibookRepo = ibookRepo;
            this._ishelfRepo = ishelfREpo;
        }

        private List<Book>? GetHaveReadShelf(int userId)
        {
            foreach(Shelf shelf in _ishelfRepo.GetUserShelves(userId))
            {
                if(shelf.Name == "Have Read")
                {
                    return shelf.Books;
                }
            }
            return null;
        }

        public List<Book> GetUnreadBooks(int userId)
        {
            List<Book>? readBooks = GetHaveReadShelf(userId);
            if(readBooks != null)
            {
                List<Book> unreadBooks = new List<Book>();
                foreach (Book book in _ibookRepo.LoadBooks())
                {
                    if (!readBooks.Contains(book))
                    {
                        unreadBooks.Add(book);
                    }
                }
                return unreadBooks;
            }
            return _ibookRepo.LoadBooks();
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
