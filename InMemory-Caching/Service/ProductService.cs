using InMemory_Caching.Data;
using InMemory_Caching.Entites;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InMemory_Caching.Service
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default); // this method is used to retrieve all products from the database asynchronously and defualt means that if the caller does not provide a cancellation token, a default one will be used which will not cancel the operation 
        Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken); // this method is used to retrieve a product by its unique identifier asynchronously

        Task<Product> CreateAsync(ProductCreationDto productCreationDto, CancellationToken cancellationToken = default); // this method is used to create a new product in the database asynchronously
    }




    public class ProductService(AppDbContext ctx, IMemoryCache cache, ILogger<ProductService> logger) : IProductService
    {
        private const string AllProductCacheKey = "products"; // this is a constant string to represent the cache key for storing all products in the in-memory cache

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default) // this method is used to retrieve all products from the database asynchronously and it first checks if the products are available in the cache, if not it retrieves them from the database and stores them in the cache for future use {}
        {
            // get or create async 

            var products = await cache.GetOrCreateAsync(AllProductCacheKey, async entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromSeconds(30)) // this line is used to set a sliding expiration for the cache entry, which means that the cache entry will expire if it has not been accessed for 30 seconds and it helps to ensure that the cache does not hold stale data for too long
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)) // this line is used to set an absolute expiration for the cache entry, which means that the cache entry will expire after 5 minutes regardless of whether it has been accessed or not and it helps to ensure that the cache does not hold stale data for too long
                .SetPriority(CacheItemPriority.High) // this line is used to set the priority of the cache entry to high, which means that it will be less likely to be removed from the cache when the cache needs to free up memory and it helps to ensure that important data remains in the cache
                .SetSize(1); // this line is used to set the size of the cache entry to 1 unit (e.g., byte, item, etc.) and it helps to keep track of the total size of the cache and allows for better control over cache usage


                logger.LogInformation("Cache miss for all products. Retrieving from database."); // this line is used to log an informational message indicating that there was a cache miss for all products and that the products are being retrieved from the database and it helps to provide insights into the cache performance and usage

                return await ctx.Products.ToListAsync(cancellationToken); // this line is used to retrieve all products from the database asynchronously using Entity Framework Core and it returns a list of products

            });
            return products ?? new List<Product>(); // this line is used to return the list of products retrieved from the cache or the database
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var cachekey = $"product_{id}"; // this line is used to create a cache key for storing a specific product in the in-memory cache based on its unique identifier


            if (cache.TryGetValue(cachekey, out Product product))
            {
                logger.LogInformation("Cache hit for product with id {id}.", id); // this line is used to log an informational message indicating that there was a cache hit for a product with the specified id and it helps to provide insights into the cache performance and usage
                return product; // this line is used to return the product retrieved from the cache
            }
            

                logger.LogInformation("Cache miss for product with id {id}. Retrieving from database.", id); // this line is used to log an informational message indicating that there was a cache miss for a product with the specified id and that the product is being retrieved from the database and it helps to provide insights into the cache performance and usage

            product = await ctx.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken); // this line is used to retrieve a product by its unique identifier from the database asynchronously using Entity Framework Core and it returns the product if found or null if not found

            if (product != null)
            {
                cache.Set(cachekey, product, new MemoryCacheEntryOptions() // this line is used to store the retrieved product in the in-memory cache with the specified cache key and cache entry options
                .SetSlidingExpiration(TimeSpan.FromSeconds(30)) // this line is used to set a sliding expiration for the cache entry, which means that the cache entry will expire if it has not been accessed for 30 seconds and it helps to ensure that the cache does not hold stale data for too long
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)) // this line is used to set an absolute expiration for the cache entry, which means that the cache entry will expire after 5 minutes regardless of whether it has been accessed or not and it helps to ensure that the cache does not hold stale data for too long
                .SetPriority(CacheItemPriority.High) // this line is used to set the priority of the cache entry to high, which means that it will be less likely to be removed from the cache when the cache needs to free up memory and it helps to ensure that important data remains in the cache
                .SetSize(1)); // this line is used to set the size of the cache entry to 1 unit (e.g., byte, item, etc.) and it helps to keep track of the total size of the cache and allows for better control over cache usage
            }

            return product; // this line is used to return the product retrieved from the database or an empty list if not found

        }

        public async Task<Product> CreateAsync(ProductCreationDto productCreationDto, CancellationToken cancellationToken = default)
        {
            var product = new Product(productCreationDto.Name, productCreationDto.Description, productCreationDto.Price); // this line is used to create a new product instance using the properties from the ProductCreationDto and it generates a new unique identifier for the product

            ctx.Products.Add(product); // this line is used to add the newly created product to the database context
            await ctx.SaveChangesAsync(cancellationToken); // this line is used to save the changes to the database asynchronously using Entity Framework Core

            cache.Remove(AllProductCacheKey); // this line is used to remove the cache entry for all products from the in-memory cache to ensure that the cache is updated with the newly created product and it helps to maintain cache consistency

            return product; // this line is used to return the newly created product
        }

    }
}
