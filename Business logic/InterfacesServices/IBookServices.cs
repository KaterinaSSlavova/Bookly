using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IBookServices
    {
        bool AddBook(BookViewModel bookModel);
        List<BookViewModel> LoadBooks();
        BookViewModel? GetBookById(int id);
        void RemoveBook(int id);
        List<SelectListItem> GetAllGenres();
    }
}
