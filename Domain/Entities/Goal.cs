using Models.Enums;

namespace Models.Entities
{
    public class Goal
    {
        public int Id { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public int ReadingGoal { get; private set; }
        public int CurrentProgress { get; private set; }
        public Status Status { get; private set; }
        public User User { get; private set; }

        public Goal(int id, DateTime start, DateTime end, int readingGoal, int currentGoal, Status status, User user)
        {
            this.Id = id;
            this.Start = start;
            this.End = end;
            this.ReadingGoal = readingGoal;
            this.CurrentProgress = currentGoal;
            this.Status = status;
            this.User = user;
        }

        public Goal(DateTime start, DateTime end, int readingGoal, User user) // for creating goal
        {
            this.Start = start;
            this.End = end.AddHours(23).AddMinutes(59);
            this.ReadingGoal = readingGoal;
            this.CurrentProgress = 0;
            this.Status = Status.Not_started;
            this.User = user;
        }

        public void SetStatus(Status status)
        {
            this.Status = status;
        }
    }
}
