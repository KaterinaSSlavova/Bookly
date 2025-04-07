using Models.Enums;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IRatingServices
    {
        bool RateBook(int bookId, int ratingId);
        List<Ratings> GetBookRatings(int bookId);
        Ratings? GetUserRatingForBook(int bookId);
        Ratings GetMostPopularRating(int bookId);
    }
}
