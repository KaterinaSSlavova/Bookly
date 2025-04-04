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
        //public List<Review> Reviews{ get; set; }

        public Book(int id, string picture, string title, string author, string description, string isbn, Genre genre)
        {
            this.Id = id;
            this.Picture = picture;
            this.Title = title;
            this.Author = author;
            this.Description = description;
            this.ISBN = isbn;
            this.Genre = genre;
        }

        public Book(string picture, string title, string author, string description, string isbn, Genre genre)
        {
            this.Picture = picture;
            this.Title = title;
            this.Author = author;
            this.Description = description;
            this.ISBN = isbn;
            this.Genre = genre;
        }
    }
}
