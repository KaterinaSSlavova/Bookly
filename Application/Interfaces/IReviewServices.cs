using Business_logic.DTOs;

namespace Interfaces
{
    public interface IReviewServices
    {
        void AddReview(string description, int bookId);
        List<ReviewDTO> GetBookReviews(BookDTO book);
        void RemoveReview(int reviewId);
    }
}
