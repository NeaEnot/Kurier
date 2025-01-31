using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Kurier.Common.Models.Entities;
using Kurier.Common.Enums;

namespace InfrastructureDB.Data.EntityConfigurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .HasMaxLength(250)
                   .IsRequired();

            builder.Property(x => x.Permissions)
                   .HasDefaultValue(UserPermissions.None)
                   .IsRequired();

            builder.Property(x => x.Email)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Password)
                   .HasMaxLength(50)
                   .IsRequired();
        }
    }
}
