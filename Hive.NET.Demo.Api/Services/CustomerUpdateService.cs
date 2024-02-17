using Hive.NET.Demo.Api.EntityModel;
using Hive.NET.Demo.Api.Repositories.Interfaces;
using Hive.NET.Demo.Api.Services.Interfaces;

namespace Hive.NET.Demo.Api.Services
{
    public class CustomerUpdateService : ICustomerUpdateService
    {
        private readonly ILogger<CustomerUpdateService> logger;
        private ICustomerRepository _customerRepository;

        public CustomerUpdateService(ILogger<CustomerUpdateService> logger)
        {
            this.logger = logger;
        }
        
        public void Initialize(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
    }
}