

namespace SoftDeletes.Api.Entites
{
    public class Category : ISoftDeletable
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public List<Product> Products {get;set;} = [];
    }

}

