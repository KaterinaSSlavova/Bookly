namespace Bookly.ViewModels
{
    public class ShelfDetailsViewModel
    {
        public RegularShelfViewModel Shelf { get; set; }
        public List<DeleteModalViewModel> DeleteModal { get; set; }
    }
}
