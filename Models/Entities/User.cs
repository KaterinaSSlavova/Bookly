using Models.Enums;

namespace Models.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public byte[] Picture { get; private set; }
        public string Username { get; private set; }
        public DateTime? BirthDate { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public Role Role { get; private set; }

        public User(int id, byte[] picture, string username, DateTime? birthDate, string email, string password, Role role)
        {
            this.Id = id;
            this.Picture = picture;
            this.Username = username;
            this.BirthDate = birthDate;
            this.Email = email;
            this.Password = password;
            this.Role = role;
        }

        public User(byte[] picture, string username, DateTime? birthDate, string email, Role role)
        {
            this.Picture = picture;
            this.Username = username;
            this.BirthDate = birthDate;
            this.Email = email;
            this.Role = role;
        }

        public User(string username, string password)  //Log in
        {
            this.Username = username;
            this.Password = password;
        }

        public User(string username, string email, string password) //Register
        {
            this.Username = username;
            this.Email = email;
            this.Password = password;
        }

        public User(string username, DateTime? birthDate, string email, Role role) //dto
        {
            this.Username = username;
            this.BirthDate = birthDate;
            this.Email = email;
            this.Role = role;
        }
    }
}
