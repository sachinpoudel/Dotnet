using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SoftDeletes.Api.Entites;

namespace SoftDeletes.Api.Entites
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter("SoftDelete", p => !p.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter("SoftDelete", p => !p.IsDeleted);

            modelBuilder.Entity<Product>().HasIndex(p => p.IsDeleted);
            modelBuilder.Entity<Category>().HasIndex(c => c.IsDeleted);

            modelBuilder.Entity<Product>()
                      .HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId);

        }
    }
}