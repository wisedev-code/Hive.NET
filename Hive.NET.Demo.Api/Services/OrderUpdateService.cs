using Hive.NET.Demo.Api.EntityModel;
using Hive.NET.Demo.Api.EntityModel.Enum;
using Hive.NET.Demo.Api.Repositories.Interfaces;
using Hive.NET.Demo.Api.Services.Interfaces;

namespace Hive.NET.Demo.Api.Services
{
    public class OrderUpdateService : IOrderUpdateService
    {
        private readonly ILogger<OrderUpdateService> logger;
        private IOrderRepository _orderRepository;

        public OrderUpdateService( ILogger<OrderUpdateService> logger)
        {
            this.logger = logger;
        }
        
        public void Initialize(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void ProcessOrder(Order order)
        {
            if (order.Customer.OrderCount > 5)
            {
                order.Status = OrderStatus.Verified;
            }
            
            if (order.Customer.DiscountAvailable && order.Customer.DiscountPercentage > 0)
            {
                order.Price = order.Price * order.Customer.DiscountPercentage / 100;
            }
        }
    }
}