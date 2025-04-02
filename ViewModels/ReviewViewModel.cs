namespace ViewModels.Model
{
    public class ReviewViewModel
    {
        public string Username { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string FormattedDate => Date.ToString("yyyy-MM-dd");
    }
}
