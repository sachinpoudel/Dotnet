namespace SoftDeletes.Api.Entites
{
    public class Product : ISoftDeletable
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public Category? Category {get;set;} = null!;
        public Guid CategoryId {get;set;}
    }
}