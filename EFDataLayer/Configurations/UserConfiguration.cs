using EFDataLayer.DBContext;
using EFDataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataLayer.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E48C32A0");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4A6F8AB57").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534B1A54B8A").IsUnique();

            entity.Property(e => e.BirthDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.RoleId)
        .HasDefaultValue((int)Role.Reader);
            entity.Property(e => e.Username).HasMaxLength(255);
        }
    }
}
