namespace Bookly.ViewModels
{
    public class CurrentBookShelfViewModel
    {
        public ShelfViewModel Shelf { get; set; }
        public List<CurrentBookViewModel> CurrentBooks { get; set; }
        public List<DeleteModalViewModel> DeleteModal { get; set; }
    }
}
