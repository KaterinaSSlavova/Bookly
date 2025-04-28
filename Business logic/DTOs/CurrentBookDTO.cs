using Models.Enums;

namespace Business_logic.DTOs
{
    public class CurrentBookDTO
    {
        public BookDTO Book { get; set; }
        public int CurrentProgress { get; set; }
        public Status Status { get; set; }

        public CurrentBookDTO(BookDTO book)
        {
            Book = book;
            CurrentProgress = 0;
            Status = Status.Not_started;
        }

        public CurrentBookDTO(BookDTO book, int progress, Status status)
        {
            this.Book = book;
            this.CurrentProgress = progress;
            this.Status = status;
        }
    }
}
