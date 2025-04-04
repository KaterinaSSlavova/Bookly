using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IReviewServices
    {
        bool AddReview(string description, BookViewModel book);
        List<ReviewViewModel> GetBookReviews(BookViewModel book);
    }
}
