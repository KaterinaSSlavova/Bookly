using EFDataLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataLayer.Configurations
{
    public class BookRatingConfiguration: IEntityTypeConfiguration<BookRating>
    {
        public void Configure(EntityTypeBuilder<BookRating> entity)
        {
            entity.HasKey(br => new { br.RatingId, br.BookId });

            entity.ToTable("BookRating");

            entity.Property(e => e.RatingId)
        .HasDefaultValue((int)Ratings.Neutral);

            entity.HasOne(br => br.Book)
                  .WithMany(b => b.BookRatings)
                  .HasForeignKey(br => br.BookId);
        }
    }
}
