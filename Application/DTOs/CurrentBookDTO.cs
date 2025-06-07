using Models.Enums;

namespace Business_logic.DTOs
{
    public class CurrentBookDTO
    {
        public BookDTO Book { get; set; }
        public UserDTO User { get; set; }
        public int CurrentProgress { get; set; }
        public Status Status { get; set; }

        public CurrentBookDTO(BookDTO book)
        {
            this.Book = book;
            CurrentProgress = 0;
            Status = Status.Not_started;
        }

        public CurrentBookDTO(BookDTO book, UserDTO user)
        {
            Book = book;
            User = user;
            CurrentProgress = 0;
            Status = Status.Not_started;
        }

        public CurrentBookDTO(BookDTO book, int progress, Status status)
        {
            Book = book;
            CurrentProgress = progress;
            Status = status;
        }

        public CurrentBookDTO()
        {
            
        }
    }
}
