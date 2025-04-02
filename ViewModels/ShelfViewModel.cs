using ViewModels.Model;

namespace ViewModels.Model
{
    public class ShelfViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookViewModel> BooksOnShelf { get; set; } = new List<BookViewModel>();
    }
}
