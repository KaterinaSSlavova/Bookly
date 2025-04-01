using Models.Entities;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IReviewServices
    {
        bool AddReview(Review review);
        List<Review> GetBookReviews(int bookId);
    }
}
