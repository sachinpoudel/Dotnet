using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace  Persistence.Configurations{
    
    public class CustomerProfileConfigurations : IEntityTypeConfiguration<Models.CustomerProfile>

    {
        public void Configure(EntityTypeBuilder<Models.CustomerProfile> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(c => c.ShippingAddress).IsRequired().HasMaxLength(200);

            builder.HasOne(p => p.Customer).WithOne(c => c.Profile)
            .HasForeignKey<Models.CustomerProfile>(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}