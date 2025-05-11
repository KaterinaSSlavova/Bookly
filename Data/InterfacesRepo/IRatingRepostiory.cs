using Models.Enums;

namespace Bookly.Data.InterfacesRepo
{
    public interface IRatingRepostiory
    {
        void RateBook(int bookId, int ratingId);
        void ConnectUserWithRating(int userId, int ratingId);
        List<Ratings> GetAllRatingsForBook(int bookId);
        Ratings? GetUserRatingForBook(int userId, int bookId);
        int CheckForRating(int userId, int bookId);
        void RemoveRating(int userId, int bookId, int ratingId);
    }
}
