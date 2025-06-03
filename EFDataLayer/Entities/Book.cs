using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataLayer.DBContext;

public partial class Book
{
    public int Id { get; set; }

    public string? Picture { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Isbn { get; set; } = null!;

    public Genre Genre { get; set; } 

    public bool IsArchived { get; set; }

    public int Pages { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<CurrentBook> UserBookProgresses { get; set; } = new List<CurrentBook>();
    public virtual ICollection<BookRating> BookRatings { get; set; } = new List<BookRating>();

    public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
}
