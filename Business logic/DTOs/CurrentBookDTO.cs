using Models.Enums;

namespace Business_logic.DTOs
{
    public class CurrentBookDTO: BookDTO
    {
        public UserDTO User { get; set; }
        public int CurrentProgress { get; set; }
        public Status Status { get; set; }

        public CurrentBookDTO(int id, string picture, string title, string author, string description, string isbn, Genre genre, int pages) 
            : base(id, picture, title, author, description, isbn, genre, pages)
        {
            this.User = null;
            CurrentProgress = 0;
            Status = Status.Not_started;
        }

        public CurrentBookDTO(int id, string picture, string title, string author, string description, string isbn, Genre genre, int pages, UserDTO user)
       : base(id, picture, title, author, description, isbn, genre, pages)
        {
            this.User = user;
            CurrentProgress = 0;
            Status = Status.Not_started;
        }

        public CurrentBookDTO(int id, string picture, string title, string author, string description, string isbn, Genre genre, 
            int pages, int progress, Status status, UserDTO user)
            : base(id, picture, title, author, description, isbn, genre, pages)
        {
            this.User = user;
            this.CurrentProgress = progress;
            this.Status = status;
        }
    }
}
