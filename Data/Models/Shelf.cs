namespace Bookly.Data.Models
{
    public class Shelf
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Book> Books { get; set; }
        public Shelf() { }
    }
}
