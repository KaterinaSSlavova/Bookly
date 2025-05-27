using EFDataLayer.DBContext;
using Interfaces;

namespace Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BooklyDbContext _context;
        public ReviewRepository(BooklyDbContext context)
        {
            _context = context;
        }

        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public List<Review> GetBookReviews(Book book)
        {
            return _context.Reviews.Where(r => r.Book.Id == book.Id && r.IsArchived == false).ToList();
        }

        public void RemoveReview(int reviewId)
        {
            Review review = _context.Reviews.Find(reviewId);
            if (review != null)
            {
                review.IsArchived = true;
                _context.Reviews.Update(review);
                _context.SaveChanges();
            }
        }
    }
}
