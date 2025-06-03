using System.ComponentModel.DataAnnotations.Schema;
using EFDataLayer.Entities;

namespace EFDataLayer.DBContext;

public partial class UserRating
{
    public int UserId { get; set; }

    public int RatingId { get; set; }

    public int Id { get; set; }

    [NotMapped]
    public virtual Ratings Rating
    {
        get => (Ratings)RatingId;
        set => RatingId = (int)value;
    }

    public virtual User User { get; set; } = null!;
}
