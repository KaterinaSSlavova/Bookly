using Models.Entities;

namespace Bookly.ViewModels
{
    public class ReviewViewModel
    {
        public string Username { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }

        public static ReviewViewModel ConvertToViewModel(Review review)
        {
            ReviewViewModel viewModel = new ReviewViewModel()
            {
                Username = review.User.Username,
                Description = review.Description,   
                Date = review.Date.ToString("yyyy-MM-dd")
            };
            return viewModel;
        }
    }
}
