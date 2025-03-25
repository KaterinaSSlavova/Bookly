namespace Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Shelf> Shelves { get; set; }
        public List<Goal> Goals { get; set; }
        public List<Review> Reviews { get; set; }

        public User()
        {
        }

        public User(int id, string picture, string username, int age, string email, string password)
        {
            this.Id= id;
            this.Picture= picture;
            this.Username= username;
            this.Age = age;
            this.Email = email;
            this.Password = password;
        }
    }
}
