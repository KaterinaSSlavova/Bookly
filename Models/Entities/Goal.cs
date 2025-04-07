using Models.Enums;

namespace Models.Entities
{
    public class Goal
    {
        public int Id { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public int ReadingGoal { get; }
        public int CurrentProgress { get; private set; }
        public Status Status { get; }
        public User User { get; private set; }

        public void SetCurrentProgress(int progress)
        {
            if(CurrentProgress >= 0 &&  progress <=  ReadingGoal)
            {
                CurrentProgress = progress;
            }
        }

        public void SetUser(User user)
        {
            this.User = user;
        }

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
    }
}
