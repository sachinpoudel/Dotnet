using Microsoft.EntityFrameworkCore;

namespace  InMemory_Caching.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
    {
        public DbSet<Entites.Product> Products { get; set; } // this is a DbSet property to represent the collection of products in the database


        protected override void OnModelCreating(ModelBuilder modelBuilder) // this method is used to configure the model and its relationships
        {
            // base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entites.Product>(entity =>
            {
                entity.HasKey(p => p.Id); // Set the primary key for the Product entity
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100); // Configure the Name property
                entity.Property(p => p.Description).HasMaxLength(500); // Configure the Description property
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)"); // Configure the Price property
            });
        }
        
        
        
    }
}