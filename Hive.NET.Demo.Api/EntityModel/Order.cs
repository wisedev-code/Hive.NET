using Hive.NET.Demo.Api.EntityModel.Enum;

namespace Hive.NET.Demo.Api.EntityModel;

public class Order
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public Customer Customer { get; set; }
    public string Product { get; set; }
    public decimal Price { get; set; }
    public OrderStatus Status { get; set; }

    public Order(string number, Customer customer, string product, decimal price)
    {
        Id = Guid.NewGuid();
        Number = number;
        Customer = customer;
        Product = product;
        Price = price;
        Status = OrderStatus.Created;
    }
}