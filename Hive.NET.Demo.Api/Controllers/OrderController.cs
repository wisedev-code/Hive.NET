using Hive.NET.Core.Components;
using Hive.NET.Core.Extensions;
using Hive.NET.Core.Manager;
using Hive.NET.Demo.Api.EntityModel;
using Hive.NET.Demo.Api.Repositories.Interfaces;
using Hive.NET.Demo.Api.RequestModel;
using Hive.NET.Demo.Api.Services;
using Hive.NET.Demo.Api.Services.Interfaces;
using Hive.NET.Extensions.Components;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hive.NET.Demo.Api.Controllers
{
    //PS: This api is not an example of properly done SOLID Web Api, its simple showcase to see how Hive.NET components can be implemented
    [SwaggerTag("Controller to work with Orders")]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderUpdateService _orderUpdateService;
        private readonly IHiveManager _hiveManager;

        public OrderController(ILogger<OrderController> logger, IOrderRepository orderRepository,
            ICustomerRepository customerRepository, IOrderUpdateService orderUpdateService, IHiveManager hiveManager)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _orderUpdateService = orderUpdateService;
            _hiveManager = hiveManager;
        }

        // GET: api/Order
        [SwaggerOperation(Summary = "Get all available orders")]
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            _logger.LogInformation("Get all orders invoked");
            var result = _orderRepository.GetAll();
            return result;
        }

        // GET: api/Order/8d35a21b-2b53-4407-8959-75910582af36
        [SwaggerOperation(Summary = "Get order by id")]
        [HttpGet("{id}")]
        public Order Get(Guid id)
        {
            _logger.LogInformation($"Get order {id} invoked");
            var result = _orderRepository.Get(id);
            return result;
        }

        // POST: api/Order
        [SwaggerOperation(Summary = "Add order to in memory database")]
        [HttpPost]
        public IActionResult Post([FromBody] OrderCreateRequest value)
        {
            _logger.LogInformation($"Create order {value.Number} invoked");
            var workItem = new BeeWorkItem(
                new Task(
                    () =>
                    {
                        var customer = _customerRepository.Get(value.CustomerId);
                        Order order = new Order(value.Number, customer, value.Product, value.Price);
                        customer.OrderCount++;
                        _orderRepository.Save(order);
                        _customerRepository.Update(customer);
            
                        _orderUpdateService.ProcessOrder(order);
                    }),
                "Add order with number " + value.Number,
                () => _logger.LogInformation($"Order with number {value.Number} added"),
                _ => _logger.LogError($"Order with number {value.Number} failed to save"));

            _hiveManager
                .GetHive(Keys.OrderHiveName)!
                .AddTask(workItem);
            
            return Accepted();
        }

        // PUT: api/Order/8d35a21b-2b53-4407-8959-75910582af36
        [SwaggerOperation(Summary = "Update order by given id (you can update only price and product")]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] OrderUpdateRequest value)
        {
            _logger.LogInformation($"Update order {id} invoked");
            var order = _orderRepository.Get(id);

            var workItem = new BeeWorkItem(
                new Task(
                    () =>
                    {
                        order.Price = value.Price;
                        order.Product = value.Product;
                        _orderRepository.Update(order);
                    }),
                "Update order with id " + id,
                () => _logger.LogInformation($"Order with id {id} updated"),
                _ => _logger.LogError($"Order with id {id} failed to update"));

            _hiveManager
                .GetHive(Keys.OrderHiveName)!
                .AddTaskWithPriority(workItem, BeeWorkItemPriority.High);
            
            return Ok(order);
        }

        // DELETE: api/Order/8d35a21b-2b53-4407-8959-75910582af36
        [SwaggerOperation(Summary = "Delete order by given id")]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation($"Delete order {id} invoked");
            
            var workItem = new BeeWorkItem(
                new Task(
                    () =>
                    {
                        _orderRepository.Delete(id);
                    }),
                "Delete order with id " + id,
                () => _logger.LogInformation($"Order with id {id} deleted"),
                _ => _logger.LogError($"Order with id {id} failed to delete"));

            _hiveManager
                .GetHive(Keys.OrderHiveName)!
                .AddTask(workItem);
            
            return NoContent();
        }
    }
}
