using EFDataLayer.DBContext;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class ShelfRepository: IShelfRepository
    {
        private readonly BooklyDbContext _context;

        public ShelfRepository(BooklyDbContext context)
        {
            _context = context;
        }

        public void CreateShelf(Shelf shelf, int id)
        {
            try
            {
                shelf.UserId = id;
                _context.Shelves.Add(shelf);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Book> GetBooksFromShelf(int id)
        {
            Shelf? shelf = _context.Shelves.Find(id);
            return shelf.Books.ToList();
        }

        public void AddBookToShelf(int bookId, int shelfId, int userId)
        {
            Shelf? shelf = _context.Shelves.Find(shelfId);
            Book? book =  _context.Books.Find(bookId);
            shelf.UserId=userId;    
            shelf.Books.Add(book);  
            _context.SaveChanges();
        }

        public void SetCurrentBookProgress(CurrentBook book)
        {
            _context.CurrentBooks.Add(book);
            _context.SaveChanges();
        }

        public void SaveCurrentBookProgress(CurrentBook book)
        {
            _context.CurrentBooks.Update(book);
            _context.SaveChanges();
        }

        public void RemoveBookFromShelf(int userId, int bookId)
        {
            Shelf? shelf = _context.Shelves.FirstOrDefault(s => s.UserId == userId && s.Books.Any(b => b.Id == bookId));
            if (shelf == null)
            {
                Book? book = shelf.Books.FirstOrDefault(b => b.Id == bookId);
                shelf.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public void RemoveFromCurrentBookShelf(int userId, int bookId)
        {
            CurrentBook? book = _context.CurrentBooks.Where(b => b.UserId == userId && b.UserId == b.UserId).FirstOrDefault();
            _context.CurrentBooks.Remove(book);
            _context.SaveChanges();
        }

        public void RemoveShelf(int id)
        {
            Shelf? shelf = _context.Shelves.Find(id);
            shelf.IsArchived = true;
            _context.SaveChanges();
        }

        public List<RegularShelf> GetUserRegularShelves(User user)
        {
            List<Shelf> shelves = _context.Shelves.Where(s => s.UserId == user.Id && s.IsArchived == false).ToList();
            shelves.ForEach(s => s.User= user); 
            return shelves.Select(s => ConvertToRegularShelf(s)).ToList();
        }

        public RegularShelf? GetShelfById(int id)
        {
            Shelf? shelf = _context.Shelves.Where(s => s.Id == id).FirstOrDefault();
            if (shelf == null)
            {
                return ConvertToRegularShelf(shelf);
            }
            return null;
        }

        public CurrentBookShelf? GetUserCurrentShelf(User user, string shelfName)
        {
            Shelf? shelf = _context.Shelves.Where(s => s.UserId == user.Id && s.Name == shelfName).FirstOrDefault();
            CurrentBookShelf currentBookShelf = ConvertToCurrentBookShelf(shelf, GetBooksFromCurrentlyReadingShelf(user));
            return currentBookShelf;
        }

        public List<CurrentBook> GetBooksFromCurrentlyReadingShelf(User user)
        {
            return _context.CurrentBooks.Include(b => b.Book).Where(b => b.UserId == user.Id).ToList();
        }

        public RegularShelf ConvertToRegularShelf(Shelf shelf)
        {
            RegularShelf regularShelf = new RegularShelf()
            {
                Shelf = shelf,  
                Books = shelf.Books
            };
            return regularShelf;    
        }

        public CurrentBookShelf ConvertToCurrentBookShelf(Shelf shelf, List<CurrentBook> books)
        {
            CurrentBookShelf currentShelf = new CurrentBookShelf()
            {
                Id= shelf.Id,
                Name = shelf.Name,
                Books = books 
            };
            return currentShelf;
        }
    }
}
