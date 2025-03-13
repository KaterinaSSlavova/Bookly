namespace Bookly.Models
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

        public Goal() { }   
    }
}
