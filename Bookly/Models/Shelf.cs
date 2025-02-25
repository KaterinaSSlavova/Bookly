namespace Bookly.Models
{
    public class Shelf
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public List<Book> Books { get; set; }
    }
}
