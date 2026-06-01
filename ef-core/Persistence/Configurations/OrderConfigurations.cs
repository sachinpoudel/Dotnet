
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
 namespace Persistence.Configurations

{
 public class OrderConfigurations : IEntityTypeConfiguration<Models.Order>
    {
        public void Configure(EntityTypeBuilder<Models.Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.OrderDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasOne(o => o.Customer).WithMany(c => c.Orders).HasForeignKey(o => o.CustomerId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany( o => o.OrderItem).WithOne(oi => oi.Order).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
        }
    }    
}