namespace Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public CustomerProfile? Profile { get; set; }
        public ICollection<Order> Orders { get; set; } = null!;
    }

    public class CustomerProfile
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public Guid CustomerId { get; set; } // fk
        public Customer Customer { get; set; } = null!;
    }

    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public Guid CustomerId { get; set; } // fk
        public Customer Customer { get; set; } = null!; // navigation property
        public ICollection<OrderItem> OrderItem = []; // relationship with OrderItem ie one order can have many order items

    }

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public Guid CategoryId {get;set;} //fk
        public Category Category {get;set;} = null!;
    
    }

    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; } //fk
        public Order Order { get; set; } = null!;
        public Guid ProductId {get;set;}

        public Product Product {get;set;} = null!;

        public int Quantity {get;set;}

    }

    public class Category
    {
        public Guid Id {get;set;}
        public string Name {get;set;} = string.Empty;
        public string Description {get;set;} = string.Empty;


        public Product Product {get;set;} = null!;
        public ICollection<Product> Products = [];

    }
}