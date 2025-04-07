using Models.Enums;

namespace Models.Entities
{
    public class Book
    {
        public int Id { get; }
        public string Picture { get; }
        public string Title { get; }
        public string Author { get; }
        public string Description { get; }
        public string ISBN { get; }
        public Genre Genre { get; }

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
      
        public Book() { }   
    }
}
