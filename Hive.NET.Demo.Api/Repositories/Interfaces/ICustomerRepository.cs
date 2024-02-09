using Hive.NET.Demo.Api.EntityModel;

namespace Hive.NET.Demo.Api.Repositories.Interfaces;

public interface ICustomerRepository
{
    public void Save(Customer customer);
    public void Update(Customer customer);
    public void Delete(Guid id);
    public Customer Get(Guid id);
    public IEnumerable<Customer> GetAll();
}