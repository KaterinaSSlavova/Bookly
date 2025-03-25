using System.ComponentModel.DataAnnotations;

namespace ViewModels.Model
{
    public class AccountLogIn
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
