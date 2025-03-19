namespace Bookly.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public List<Review> Reviews{ get; set; }
        public List<Emotion> Emotions { get; set; }
    }
}
