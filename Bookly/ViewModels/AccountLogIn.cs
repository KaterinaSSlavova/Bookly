using System.ComponentModel.DataAnnotations;

namespace Bookly.ViewModels
{
    public class AccountLogIn
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
