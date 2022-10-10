using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Accounting.Data;
using Accounting.Application.Models;
using Accounting.Application.Services;
using Accounting.Domain;
using Microsoft.AspNetCore.Http;

namespace Accounting.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerRepository _repository;

        public CustomerController(ICustomerRepository context, ILogger<CustomerController> logger, IHttpContextAccessor httpContext) : base()
        {
            _logger = logger;
            _repository = context;

            _repository.FindUser(httpContext.HttpContext!.User.Identity!.Name!);

            // Lookup current User
            if (_repository.User is null)
            {
                throw new CustomApplicationException("User not found");
            }
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
        {
            var customers = await _repository.GetAllCustomersByUser();

            if (customers is null)
            {
                return NotFound(customers);
            }
            return Ok(await Mapper.Map((List<Customer>)customers));
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {

            var customer = await _repository.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map(customer));
        }

        // PUT: api/Customer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (id != customerDto.Id)
            {
                return BadRequest();
            }

            await _repository.UpdateCustomer(id, Mapper.Map(customerDto));
            return NoContent();
        }

        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerDto customerDto)
        {
            // Map the new Customer and add them to the Invoice
            var customerId = await _repository.CreateCustomer(Mapper.Map(customerDto));
            return CreatedAtAction("GetCustomer", new { id = customerId }, customerDto);
        }
    }
}
