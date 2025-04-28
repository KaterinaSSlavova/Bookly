namespace Bookly.ViewModels
{
    public class CurrentBookShelfViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CurrentBookViewModel> BooksOnShelf { get; set; }
    }
}
