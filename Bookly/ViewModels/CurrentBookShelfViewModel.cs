namespace Bookly.ViewModels
{
    public class CurrentBookShelfViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public List<CurrentBookViewModel> CurrentBooks { get; set; }
    }
}
