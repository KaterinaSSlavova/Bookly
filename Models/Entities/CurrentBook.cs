using Models.Enums;

namespace Models.Entities
{
    public class CurrentBook
    {
        public int Id { get; set; } 
        public int CurrentProgress { get; private set; }
        public Status Status { get; private set; }

        public CurrentBook(int id, int progress, Status status)
        {
            this.Id = id;
            this.CurrentProgress = progress;
            this.Status = status;
        }

        public CurrentBook() { }
    }
}
