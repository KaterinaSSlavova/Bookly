namespace Bookly.Models
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
    }
}
