using EFDataLayer.DBContext;
using Interfaces;
using Microsoft.Extensions.Logging;

namespace Repositories
{
    public class BookRepository: IBookRepository
    {
        private readonly BooklyDbContext _context;
        //private readonly ILogger _logger;   
        public BookRepository(BooklyDbContext context)
        {
            _context = context;
           // _logger = logger;
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public List<Book> LoadBooks()
        {
            //try
            //{
                return _context.Books.Where(b => b.IsArchived != true).ToList();
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message, "Error when loading books");
            //    return new List<Book>();
            //}
        }

        public Book? GetBookById(int id)
        {
            return _context.Books.Find(id);
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void RemoveBook(int id)
        {
            Book? book = _context.Books.Find(id);
            if (book != null)
            {
                book.IsArchived = false;
                _context.Books.Update(book);
                _context.SaveChanges();
            }
        }
    }
}
