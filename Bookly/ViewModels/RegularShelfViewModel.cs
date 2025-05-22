namespace Bookly.ViewModels
{
	public class RegularShelfViewModel
	{
        public ShelfViewModel Shelf { get; set; }
        public List<BookViewModel> BooksOnShelf { get; set; }
	}
}
