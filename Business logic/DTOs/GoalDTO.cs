using Models.Enums;

namespace Business_logic.DTOs
{
    public class GoalDTO
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int ReadingGoal { get; set; }
        public int CurrentProgress { get; set; }
        public Status Status { get; set; }
        public UserDTO User { get; set; }

        public GoalDTO(int id, DateTime start, DateTime end, int readingGoal, int currentProgress, Status status, UserDTO user)
        {
            this.Id = id;
            this.Start = start;
            this.End = end;
            this.ReadingGoal = readingGoal;
            this.CurrentProgress = currentProgress;
            this.Status = status;
            this.User = user;
        }

        public GoalDTO()
        { 
        }
    }
}
