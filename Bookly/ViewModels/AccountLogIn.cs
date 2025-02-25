using System.ComponentModel.DataAnnotations;

namespace Bookly.ViewModels
{
    public class AccountLogIn
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
