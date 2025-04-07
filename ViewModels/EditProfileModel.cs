using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ViewModels.Model
{
    public class EditProfileModel
    {
        [Required]
        public IFormFile Picture { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
