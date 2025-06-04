using Microsoft.EntityFrameworkCore;
using EFDataLayer.DBContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataLayer.Configurations
{
    public class BookConfiguration: IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC07FEA26F79");

            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.Genre)
        .HasConversion<string>()
        .HasMaxLength(255);
            entity.Property(e => e.IsArchived).HasColumnName("isArchived");
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.Picture).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        }
    }
}
