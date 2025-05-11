using Models.Entities;

namespace Bookly.Data.InterfacesRepo
{
    public interface IReviewRepository
    {
        void AddReview(Review review);
        List<Review> GetBookReviews(Book book);
        void RemoveReview(int reviewId);
    }
}
