using Models.Enums;

namespace Models.Entities
{
    public class CurrentBook
    {
        public Book Book {  get; private set; }
        public User User { get; private set; }  
        public int CurrentProgress { get; private set; }
        public Status Status { get; private set; }

        public CurrentBook(Book book, User user, int currentProgress, Status status)
        {
            this.Book = book;
            this.User = user;
            this.CurrentProgress = currentProgress;
            this.Status = status;
        }

        public CurrentBook() { }
    }
}
