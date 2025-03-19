using System.ComponentModel.DataAnnotations;

namespace Bookly.Data.ViewModels
{
    public class AccountRegister
    {
        [Required]
        public string Username {  get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
