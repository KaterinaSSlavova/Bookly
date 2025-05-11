using Business_logic.DTOs;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IReviewServices
    {
        void AddReview(string description, int bookId);
        List<ReviewDTO> GetBookReviews(BookDTO book);
        void RemoveReview(int reviewId);
    }
}
