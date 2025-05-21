namespace Business_logic.DTOs
{
    public class ReviewDTO
    {
        public int Id { get;  set; }
        public string Description { get;  set; }
        public DateTime Date { get; set; }
        public UserDTO User { get; set; }
        public BookDTO Book { get; set; }

        public ReviewDTO(string description, UserDTO user, BookDTO book)
        {
            this.Description = description;
            this.Date = DateTime.Today;
            this.User = user;
            this.Book = book;
        }

        public ReviewDTO(int id, string description, DateTime date, UserDTO user, BookDTO book)
        {
            this.Id = id;
            this.Description=description;
            this.Date = date;
            this.User = user;
            this.Book = book;
        }
    }
}
