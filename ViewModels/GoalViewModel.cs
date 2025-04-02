namespace ViewModels.Model
{
    public class GoalViewModel
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public string FormattedStartDate => Start.ToString("yyyy-MM-dd");
        public DateTime End { get; set; }
        public string FormattedEndDate => End.ToString("yyyy-MM-dd");
        public int ReadingGoal {  get; set; }
        public int CurrentProgress { get; set; }
        public string Status { get; set; }
    }
}
