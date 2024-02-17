using Hive.NET.Demo.Api.EntityModel;

namespace Hive.NET.Demo.Api.Repositories.Interfaces;

public interface IOrderRepository
{
    public void Save(Order order);
    public void Update(Order order);
    public void Delete(Guid id);
    public Order Get(Guid id);
    public IEnumerable<Order> GetAll();
}