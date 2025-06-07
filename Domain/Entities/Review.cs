namespace Models.Entities
{
    public class Review
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public DateTime Date { get; private set; }
        public User User { get; private set; }
        public Book Book { get; private set; }

        public Review(string description, DateTime date, User user, Book book)
        {
            this.Description = description;
            this.Date = date;
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
