using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Kurier.Common.Models.Entities;
using Kurier.Common.Enums;

namespace InfrastructureDB.Data.EntityConfigurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DeliveryAddress)
                   .HasMaxLength(250)
                   .IsRequired();

            builder.Property(x => x.DepartureAddress)
                  .HasMaxLength(250)
                  .IsRequired();

            builder.Property(x => x.Status)
                   .HasDefaultValue(OrderStatus.Created)
                  .IsRequired();

            builder.Property(x => x.Created)
                  .IsRequired();

            builder.Property(x => x.Weight)
                  .IsRequired();

            builder.Property(x => x.LastUpdate)
                  .IsRequired();

            builder.Property(x => x.UserId)
                  .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
