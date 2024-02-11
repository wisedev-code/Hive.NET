using Hive.NET.Core.Components;
using Hive.NET.Core.Manager;
using Hive.NET.Demo.Api.EntityModel;
using Hive.NET.Demo.Api.Repositories.Interfaces;
using Hive.NET.Demo.Api.RequestModel;
using Hive.NET.Extensions.Components;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hive.NET.Demo.Api.Controllers
{
    //PS: This api is not an example of properly done SOLID Web Api, its simple showcase to see how Hive.NET components can be implemented
    [SwaggerTag("Controller to work with Customers")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<OrderController> _logger;
        private readonly IHiveManager _hiveManager;

        public CustomerController(
            ICustomerRepository customerRepository, 
            ILogger<OrderController> logger, 
            IHiveManager hiveManager)
        {
            _customerRepository = customerRepository;
            _logger = logger;
            _hiveManager = hiveManager;
        }

        // GET: api/Customer
        [SwaggerOperation(Summary = "Get all available customers")]
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            _logger.LogInformation("Get all customers invoked");
            var result = _customerRepository.GetAll();
            return result;
        }

        // GET: api/Customer/8d35a21b-2b53-4407-8959-75910582af36
        [SwaggerOperation("Get customer by id")]
        [HttpGet("{id}")]
        public Customer Get(Guid id)
        {
            _logger.LogInformation($"Get customer {id} invoked");
            var result = _customerRepository.Get(id);
            return result;
        }

        // POST: api/Customer
        [SwaggerOperation("Create new customer")]
        [HttpPost]
        public IActionResult Post([FromBody] CustomerCreateRequest customerCreateRequest)
        {
            _logger.LogInformation($"Create customer {customerCreateRequest.FirstName} | {customerCreateRequest.LastName} invoked");
            
            var workItem = new BeeWorkItem(
                new Task(() =>
                {
                    var customer = new Customer(customerCreateRequest.FirstName, customerCreateRequest.LastName);
                    _customerRepository.Save(customer);
                }),
                "Create customer task",
                () => _logger.LogInformation($"Create customer {customerCreateRequest.FirstName}|{customerCreateRequest.LastName} task completed", 
                    () => _logger.LogError($"Create customer {customerCreateRequest.FirstName}|{customerCreateRequest.LastName} task failed")));

            _hiveManager
                .GetHive(Keys.CustomerHiveName)!
                .AddTask(workItem);
          
            return NoContent();
        }

        // PUT: api/Customer/8d35a21b-2b53-4407-8959-75910582af36
        [SwaggerOperation("Update existing customer - You can update customer by adding discount or removing existing one. Adding discount will only be possible if customer make already at least 10 orders")]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] CustomerDiscountRequest customerDiscount)
        {
            _logger.LogInformation($"Update customer {id} invoked");
            var customer = _customerRepository.Get(id);

            var workItem = new BeeWorkItem(
                new Task(() =>
                {
                    if (customerDiscount.CanHaveDiscount && customer.DiscountAvailable)
                    {
                        customer.GiveDiscount(customerDiscount.Discount);
                    }
                    else
                    {
                        customer.RemoveDiscount();
                    }
                    _customerRepository.Update(customer);
                }),
                "Update customer task",
                () => _logger.LogInformation($"Update customer {id} task completed", 
                    () => _logger.LogError($"Update customer {id} task failed")));

            _hiveManager
                .GetHive(Keys.CustomerHiveName)!
                .AddTaskWithPriority(workItem, BeeWorkItemPriority.High);

            return Accepted(customer);
        }

        // DELETE: api/Customer/8d35a21b-2b53-4407-8959-75910582af36
        [SwaggerOperation(Summary = "Delete customer by id")]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var workItem = new BeeWorkItem(
                new Task(() =>
                {
                    _customerRepository.Delete(id);
                }),
                "Delete customer task",
                () => _logger.LogInformation($"Delete customer {id} task completed", 
                    () => _logger.LogError($"Delete customer {id} task failed")));
            
            _hiveManager
                .GetHive(Keys.CustomerHiveName)!
                .AddTask(workItem);
            
            return NoContent();
        }
    }
}