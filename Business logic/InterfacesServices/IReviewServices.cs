using Business_logic.DTOs;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IReviewServices
    {
        bool AddReview(string description, int bookId);
        List<ReviewDTO> GetBookReviews(BookDTO book);
    }
}
