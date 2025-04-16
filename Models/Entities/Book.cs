using Models.Enums;

namespace Models.Entities
{
    public class Book
    {
        public int Id { get; private set; }
        public string Picture { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Description { get; private set; }
        public string ISBN { get; private set; }
        public Genre Genre { get; private set; }

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
      
        public Book()
        {
            
        }
    }
}
