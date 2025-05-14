using System.ComponentModel.DataAnnotations;

namespace Bookly.ViewModels
{
    public class AccountRegister
    {
        [Required]
        public string Username {  get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
		[Compare("ConfirmPassword", ErrorMessage = "Passwords do not match.")]
		public string Password { get; set; }

        [Required]
		[Compare("Password", ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; }
    }
}
