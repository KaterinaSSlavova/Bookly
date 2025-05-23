using EFDataLayer.DBContext;
using Interfaces;

namespace Repositories
{
    public class BookRepository: IBookRepository
    {
        private readonly BooklyDbContext _context;
        public BookRepository(BooklyDbContext context)
        {
            _context = context;
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public List<Book> LoadBooks()
        {
            return _context.Books.Where(b => b.IsArchived != true).ToList();
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
