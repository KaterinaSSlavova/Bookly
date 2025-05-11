using Models.Enums;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IRatingServices
    {
        void RateBook(int bookId, int ratingId);
        List<Ratings> GetBookRatings(int bookId);
        Ratings? GetUserRatingForBook(int bookId);
        Ratings GetMostPopularRating(int bookId);
    }
}
