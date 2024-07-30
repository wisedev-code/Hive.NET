namespace Hive.NET.Demo.Api;

using EntityModel;
using Repositories.Interfaces;

public class DataSeedService : IHostedService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;

    public DataSeedService(ICustomerRepository customerRepository, IOrderRepository orderRepository)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var customer in _customers)
        {
            _customerRepository.Save(customer);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static List<Customer> _customers = new()
    {
        new Customer("John", "Smith"),
        new Customer("Florence", "Davis"),
        new Customer("Stacy", "Williams"),
        new Customer("Michael", "Cruz")
    };
}
