using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataLayer.DBContext
{
    public class BookRating
    {
        public int RatingId { get; set; }
        public int BookId { get; set; }

        [NotMapped]
        public Ratings Rating
        {
            get => (Ratings)RatingId;
            set => RatingId = (int)value;
        }

        public virtual Book Book { get; set; } = null!;
    }
}
