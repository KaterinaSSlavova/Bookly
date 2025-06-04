using EFDataLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataLayer.Configurations
{
    public class UserRatingConfiguration: IEntityTypeConfiguration<UserRating>
    {
        public void Configure(EntityTypeBuilder<UserRating> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRati__3214EC07DA49AE34");

            entity.ToTable("UserRating");

            entity.HasOne(d => d.User).WithMany(p => p.UserRatings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRatin__UserI__151B244E");
        }

    }
}
