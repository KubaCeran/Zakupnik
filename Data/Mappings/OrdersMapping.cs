using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zakupnik.Data.Entities;

namespace Zakupnik.Data.Mappings
{
    public class OrdersMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.OrderId);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
        }
    }
}
