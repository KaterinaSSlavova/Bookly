using Microsoft.AspNetCore.Http;

namespace ViewModels.Model
{
    public class EditProfileModel
    {
        public IFormFile Picture { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
}
