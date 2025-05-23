using EFDataLayer.DBContext;

namespace Interfaces
{
    public interface IReviewRepository
    {
        void AddReview(Review review);
        List<Review> GetBookReviews(Book book);
        void RemoveReview(int reviewId);
    }
}
