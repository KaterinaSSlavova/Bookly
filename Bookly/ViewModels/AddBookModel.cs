using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Enums;

namespace Bookly.ViewModels
{
    public class AddBookModel
    {
        public string Picture { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public int Pages { get; set; }
        public List<SelectListItem> Genres { get; set; } 

        public AddBookModel()
        {
            Genres = Enum.GetValues(typeof(Genre))
                         .Cast<Genre>()
                         .Select(g => new SelectListItem
                         {
                             Value = g.ToString(),
                             Text = g.ToString()
                         })
                         .ToList();
        }
    }
}
