using EFDataLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataLayer.Configurations
{
    public class CurrentBookConfiguration: IEntityTypeConfiguration<CurrentBook>
    {
        public void Configure(EntityTypeBuilder<CurrentBook> entity)
        {
            entity.HasKey(e => new { e.UserId, e.BookId }).HasName("PK_UserBook");

            entity.ToTable("UserBookProgress");

            entity.Property(e => e.StatusId)
        .HasDefaultValue((int)Status.Not_started);

            entity.HasOne(d => d.Book).WithMany(p => p.UserBookProgresses)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book");

            entity.HasOne(d => d.User).WithMany(p => p.UserBookProgresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User");
        }
    }
}
