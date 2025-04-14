using Models.Entities;

namespace Business_logic.DTOs
{
    public class ReviewDTO
    {
        public int Id { get;  set; }
        public string Description { get;  set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }

        public ReviewDTO(string description, User user, Book book)
        {
            this.Description = description;
            this.Date = DateTime.Today;
            this.User = user;
            this.Book = book;
        }

        public ReviewDTO(int id, string description, DateTime date, User user, Book book)
        {
            this.Id = id;
            this.Description=description;
            this.Date = date;
            this.User = user;
            this.Book = book;
        }
    }
}
