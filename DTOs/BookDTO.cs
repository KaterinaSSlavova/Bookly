using Models.Enums;

namespace Business_logic.DTOs
{
    public class BookDTO
    {
        public int Id { get; set;  }
        public string Picture { get; set;  }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public Genre Genre { get; set; }
        public int Pages { get; set; }

        public BookDTO(int id, string picture, string title, string author, string description, string isbn, Genre genre, int pages)
        {
            this.Id = id;
            this.Picture = picture;
            this.Title = title;
            this.Author = author;
            this.Description = description;
            this.ISBN = isbn;
            this.Genre = genre;
            this.Pages = pages;
        }

        //public BookDTO() { }
    }
}
