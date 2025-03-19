using System.ComponentModel.DataAnnotations;

namespace Bookly.Data.ViewModels
{
    public class AccountLogIn
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
