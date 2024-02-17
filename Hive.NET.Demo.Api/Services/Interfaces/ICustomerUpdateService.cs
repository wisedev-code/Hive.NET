using Hive.NET.Demo.Api.Repositories.Interfaces;

namespace Hive.NET.Demo.Api.Services.Interfaces
{
    public interface ICustomerUpdateService
    {
        void Initialize(ICustomerRepository customerRepository);
    }
}