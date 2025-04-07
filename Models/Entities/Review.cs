namespace Models.Entities
{
    public class Review
    {
        public int Id { get; }
        public string Description { get; }
        public DateTime Date { get; }
        public User User { get; }
        public Book Book { get; }

        public Review(string description, User user, Book book)
        {
            this.Description = description;
            this.Date = DateTime.Today;
            this.User = user;
            this.Book = book;
        }
        public Review(int id, string description, DateTime date, User user, Book book)
        {
            this.Id = id;
            this.Description = description;
            this.Date = date;
            this.User = user;
            this.Book = book;
        }
    }
}
