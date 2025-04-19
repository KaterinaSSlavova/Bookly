namespace Business_logic.DTOs
{
    public class UserDTO
    {
        public int Id { get;  set; }
        public string Picture { get;  set; }
        public string Username { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserDTO(int id, string picture, string username, DateTime? birthDate, string email, string password)
        {
            this.Id = id;
            this.Picture = picture;
            this.Username = username;
            this.BirthDate = birthDate;
            this.Email = email;
            this.Password = password;
        }

        public UserDTO(string picture, string username, DateTime? birthDate, string email, string password)
        {
            this.Picture = picture;
            this.Username = username;
            this.BirthDate = birthDate;
            this.Email = email;
            this.Password = password;
        }

        public UserDTO(string username, DateTime? birthDate, string email) 
        {
            this.Username = username;
            this.BirthDate = birthDate;
            this.Email = email;
        }

        public UserDTO(string username, string password)
        {
            this.Username=username;
            this.Password = password;
        }

        public UserDTO(string username, string email, string password)
        {
            this.Username = username;
            this.Email = email;
            this.Password = password;
        }
    }
}
