using EFDataLayer.DBContext;
using Interfaces;

namespace Repositories
{
    public class RatingRepository : IRatingRepostiory
    {
        private readonly BooklyDbContext _context;
        public RatingRepository(BooklyDbContext context)
        {
            _context = context;
        }

        public void RateBook(int bookId, int ratingId)
        {
            BookRating bookRating = new BookRating() { BookId = bookId, Book = _context.Books.Find(bookId), Rating = (Ratings)ratingId };
            _context.BookRatings.Add(bookRating);
            _context.SaveChanges();
        }

        public void ConnectUserWithRating(int userId, int ratingId)
        {
            UserRating userRating = new UserRating() { UserId = userId, User = _context.Users.Find(userId), Rating = (Ratings)ratingId };
            _context.UserRatings.Add(userRating);
            _context.SaveChanges();
        }

        public List<Ratings> GetAllRatingsForBook(int bookId)
        {
            return _context.BookRatings.Where(br => br.BookId == bookId).Select(br => br.Rating).ToList();
        }

        public Ratings? GetUserRatingForBook(int userId, int bookId)
        {
            int id = (from ur in _context.UserRatings
                      join br in _context.BookRatings on ur.RatingId equals br.RatingId
                      where ur.UserId == userId && br.BookId == bookId
                      select ur.RatingId).FirstOrDefault();
            if (id != null && id != 0)
                return (Ratings)id;
            else return null;
        }

        public int CheckForRating(int userId, int bookId)
        {
            return _context.UserRatings
    .Join(_context.BookRatings,
          ur => ur.RatingId,
          br => br.RatingId,
          (ur, br) => new { ur, br })
    .Count(x => x.ur.UserId == userId && x.br.BookId == bookId);
        }

        public void RemoveRating(int userId, int bookId, int ratingId)
        {
            UserRating? userRating = _context.UserRatings
        .FirstOrDefault(ur => ur.UserId == userId && ur.RatingId == ratingId);
            _context.UserRatings.Remove(userRating);

            BookRating? bookRating = _context.BookRatings.FirstOrDefault(br => br.BookId == bookId && br.RatingId == ratingId);
            _context.BookRatings.Remove(bookRating);
            _context.SaveChanges();
        }
    }
}
