using System.ComponentModel.DataAnnotations;

public record CreateProductRequest (
    [Required, StringLength(100 , MinimumLength = 1)] string Name,
    [Required, StringLength(100, MinimumLength =1)]  string Category,
    [Range(0.01, 9999)] decimal Price,
    [Range(0, int.MaxValue)] int Stock
);

public record UpdateProductRequest (
    [Required, StringLength(100 , MinimumLength = 1)] string Name,
    [Required, StringLength(100, MinimumLength =1)]  string Category,
    [Range(0.01, 9999)] decimal Price,
    [Range(0, int.MaxValue)] int Stock
);

public record UpdateStockRequest (
    [Range(0, int.MaxValue)] int Stock
);