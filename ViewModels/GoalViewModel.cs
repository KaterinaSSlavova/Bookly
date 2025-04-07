namespace ViewModels.Model
{
    public class GoalViewModel
    {
        public int Id { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int ReadingGoal {  get; set; }
        public int CurrentProgress { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
    }
}
