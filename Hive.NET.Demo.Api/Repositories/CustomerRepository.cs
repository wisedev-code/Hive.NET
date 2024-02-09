using System.Collections.Concurrent;
using Hive.NET.Demo.Api.EntityModel;
using Hive.NET.Demo.Api.Repositories.Interfaces;
using Hive.NET.Demo.Api.Services.Interfaces;

namespace Hive.NET.Demo.Api.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ConcurrentDictionary<Guid, Customer> _customers = new();
        private readonly ICustomerUpdateService _customerUpdateService;

        public CustomerRepository(ICustomerUpdateService customerUpdateService)
        {
            _customerUpdateService = customerUpdateService;
            _customerUpdateService.Initialize(this);
        }

        public void Save(Customer customer)
        {
            _customers.TryAdd(customer.Id, customer);
        }

        public void Update(Customer customer)
        {
            if (!_customers.ContainsKey(customer.Id))
            {
                throw new KeyNotFoundException();
            }

            _customers[customer.Id] = customer;
        }

        public void Delete(Guid id)
        {
            if (!_customers.ContainsKey(id))
            {
                throw new KeyNotFoundException();
            }

            _customers.Remove(id, out _);
        }

        public Customer Get(Guid id)
        {
            if (!_customers.ContainsKey(id))
            {
                throw new KeyNotFoundException();
            }

            return _customers[id];
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customers.Values;
        }
    }
}