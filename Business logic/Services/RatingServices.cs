using Models.Enums;

using Bookly.Data.Repository;

namespace Business_logic.Services
{
    public class RatingServices
    {
        private readonly RatingRepository _ratingRepository;    
        public RatingServices(RatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public bool RateBook(int userId, int bookId, int ratingId)
        {
            if(_ratingRepository.RateBook(bookId, ratingId) && _ratingRepository.ConnectUserWithRating(userId, ratingId))
            {
                return true;
            }
            return false;
        }

        public List<Ratings> GetBookRatings(int bookId)
        {
            return _ratingRepository.GetAllRatingsForBook(bookId);
        }

        public Ratings? GetUserRatingForBook(int userId, int bookId)
        {
            return _ratingRepository.GetUserRatingForBook(userId, bookId);
        }
    }
}
