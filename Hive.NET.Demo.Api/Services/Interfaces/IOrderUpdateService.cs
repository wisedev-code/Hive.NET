using Hive.NET.Demo.Api.EntityModel;
using Hive.NET.Demo.Api.Repositories.Interfaces;

namespace Hive.NET.Demo.Api.Services.Interfaces
{
    public interface IOrderUpdateService
    {
        void Initialize(IOrderRepository orderRepository);
        void ProcessOrder(Order order);
    }
}