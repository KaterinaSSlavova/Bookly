namespace Bookly.Models
{
    public class Emotion
    {
        public int Id { get; set; }
        public string Feeling { get; set; }
        public List<Book> Books { get; set; }
    }
}
