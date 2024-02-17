namespace Hive.NET.Demo.Api.RequestModel;

public class CustomerDiscountRequest
{
    public bool CanHaveDiscount { get; set; }
    public int Discount { get; set; }
}