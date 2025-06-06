using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataLayer.DBContext
{
    public class ReviewConfiguration: IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3214EC07C65679B4");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.IsArchived).HasColumnName("isArchived");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reviews__BookId__45F365D3");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reviews__UserId__44FF419A");
        }
    }
}
