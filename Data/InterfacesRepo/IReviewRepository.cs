using Models.Entities;

namespace Bookly.Data.InterfacesRepo
{
    public interface IReviewRepository
    {
        bool AddReview(Review review);
        List<Review> GetBookReviews(int bookId);
    }
}
