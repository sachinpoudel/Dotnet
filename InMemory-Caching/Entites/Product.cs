namespace InMemory_Caching.Entites
{
    public class Product {

      public  Guid Id { get; set; }
    public string Name {get;set;} =string.Empty;
    public string Description {get;set;} =string.Empty;
  public   decimal Price {get;set;}


    private Product() { } // this is private constructor to prevent direct instantiation

public Product(string name, string description, decimal price) // this is a public constructor to create a new product with the specified properties
        {
            Id = Guid.NewGuid(); // Generate a new unique identifier
            Name = name;
            Description = description;
            Price = price;
        }

    }
    public record ProductCreationDto(string Name, string Description, decimal Price); // this is a record type to represent the data transfer object for creating a new product
}