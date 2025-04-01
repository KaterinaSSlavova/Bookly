using Models.Enums;

namespace Models.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public Genre Genre { get; set; }
        public List<Review> Reviews{ get; set; }
    }
}
