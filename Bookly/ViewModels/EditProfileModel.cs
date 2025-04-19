using System.ComponentModel.DataAnnotations;

namespace Bookly.ViewModels
{
    public class EditProfileModel
    {
        [Required]
        public IFormFile Picture { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
