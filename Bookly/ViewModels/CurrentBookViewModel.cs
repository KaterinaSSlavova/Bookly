namespace Bookly.ViewModels
{
    public class CurrentBookViewModel
    {
        public BookViewModel Book { get; set; }
        public int CurrentProgress { get; set; }
        public string Status { get; set; }
    }
}
