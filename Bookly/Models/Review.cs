namespace Bookly.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
