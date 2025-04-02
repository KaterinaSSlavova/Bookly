using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.InterfacesRepo;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Enums;

namespace Business_logic.Services
{
    public class RatingServices : IRatingServices
    {
        private readonly IRatingRepostiory _ratingRepository;
        public RatingServices(IRatingRepostiory ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public bool RateBook(int userId, int bookId, int ratingId)
        {
            int previousRatingCount = _ratingRepository.CheckForRating(userId, bookId);
            if (previousRatingCount > 0)
            {
                _ratingRepository.RemoveRating(userId, bookId, ratingId);
            }
            if (_ratingRepository.RateBook(bookId, ratingId) && _ratingRepository.ConnectUserWithRating(userId, ratingId))
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

        public List<SelectListItem> GetAllRatings()
        {
            List<SelectListItem> ratings = Enum.GetValues(typeof(Ratings))
                .Cast<Ratings>()
                .Select(r => new SelectListItem
                {
                    Value = r.ToString(),
                    Text = r.ToString()
                })
                .ToList();
            return ratings;
        }

        public Ratings GetMostPopularRating(int bookId)
        {
            List<Ratings> bookRatings = GetBookRatings(bookId);
            Ratings rating = bookRatings.GroupBy(r => r).
                OrderByDescending(gr => gr.Count()).Select(gr => gr.Key).FirstOrDefault();
            return rating;
        }


    }
}
