using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookly.Data.InterfacesRepo;
using Models.Entities;

using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.Repository;

namespace Bookly.Business_logic.Services
{
    public class ReviewServices
    {
        private readonly ReviewRepository _reviewRepo;
        public ReviewServices(ReviewRepository reviewRepo)
        {
            this._reviewRepo = reviewRepo;
        }

        public bool AddReview(Review review)
        {
            return _reviewRepo.AddReview(review);    
        }

        public List<Review> GetBookReviews(int bookId)
        {
           return _reviewRepo.GetBookReviews(bookId);
        }
    }
}
