using Models.Enums;

namespace Models.Entities
{
    public class Goal
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int ReadingGoal { get; set; }
        public int CurrentProgress { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }

        public Goal(int id, DateTime start, DateTime end, int readingGoal, int currentGoal, Status status, User user)
        {
            this. Id = id;
            this. Start = start;
            this. End = end;
            this. ReadingGoal = readingGoal;
            this.CurrentProgress = currentGoal;
            this.Status = status;
            this.User = user;
        }
        public Goal(DateTime start, DateTime end, int readingGoal, int currentGoal, Status status, User user)
        {
            this.Start = start;
            this.End = end;
            this.ReadingGoal = readingGoal;
            this.CurrentProgress = currentGoal;
            this.Status = status;
            this.User = user;
        }

        //public Goal()
        //{

        //}
    }
}
