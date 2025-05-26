using Microsoft.AspNetCore.Mvc.Rendering;
using EFDataLayer.DBContext;

namespace Bookly.ViewModels
{
    public class AddBookModel
    {
        public int Id { get; set; }
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

        public AddBookModel(int id, string picture, string title, string author, string description, string isbn, string genre, int pages)
        {
            this.Id = id;
            this.Picture = picture;
            this.Title = title;
            this.Author = author;
            this.Description = description;
            this.ISBN = isbn;
            this.Genre = genre; 
            this.Pages = pages;
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
