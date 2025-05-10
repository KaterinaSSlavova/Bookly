using Models.Enums;

namespace Models.Entities
{
    public class CurrentBook: Book
    {
        public User User { get; private set; }  
        public int CurrentProgress { get; private set; }
        public Status Status { get; private set; }

        public CurrentBook(User user, int bookId, string picture, string title, 
            string author, string description, string isbn, Genre genre, int pages,
            int progress, Status status): 
            base(bookId, picture, title,author, description, isbn, genre, pages)
        {
            this.User = user;   
            this.CurrentProgress = progress;
            this.Status = status;
        }

        public CurrentBook() { }
    }
}
