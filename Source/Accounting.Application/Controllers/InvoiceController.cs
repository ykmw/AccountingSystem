using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accounting.Data;
using Accounting.Domain;
using Microsoft.Extensions.Logging;
using Accounting.Application.Models;
using Accounting.Application.Services;
using Microsoft.AspNetCore.Http;

namespace Accounting.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IInvoiceRepository _repository;
        private readonly User? _user;

        public InvoiceController(IInvoiceRepository context, ILogger<InvoiceController> logger, IHttpContextAccessor httpContext) : base()
        {
            _logger = logger;
            _repository = context;

            // Lookup current User
            _user = _repository.FindUser(httpContext.HttpContext!.User.Identity!.Name!);

            if (_user is null)
            {
                throw new CustomApplicationException("User not found");
            }
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAllInvoices()
        {
            var invoices = await _repository.GetAllInvociesByOrgId(_user!.Organisation.Id);

            if (invoices is null)
            {
                return NotFound(invoices);
            }

            var invoicesDto = await Mapper.Map((List<Invoice>)invoices);
            return Ok(invoicesDto);
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
        {
            var invoice = await _repository.GetInvoiceById(id);

            if (invoice == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map(invoice));
        }

        // PUT: api/Invoices/SetStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("SetStatus/{id}")]
        public async Task<IActionResult> UpdateInvoiceStatus(int id, int status)
        {
            await _repository.UpdateInvoiceStatus(id, status);
            return NoContent();
        }

        // PUT: api/Invoices/Update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, InvoiceDto invoiceDto)
        {
            if (id != invoiceDto.Id)
            {
                return BadRequest();
            }

            var invoice = await _repository.UpdateInvoice(id, Mapper.Map(invoiceDto));
            return Ok(Mapper.Map(invoice));
        }

        // POST: api/Invoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice(InvoiceDto invoiceDto)
        {
            // Map the new Invoice and add it.
            var invocieId = await _repository.CreateInvoice(Mapper.Map(invoiceDto));
            return CreatedAtAction("GetInvoice", new { id = invocieId }, Mapper.Map(invoiceDto));
        }
    }
}
