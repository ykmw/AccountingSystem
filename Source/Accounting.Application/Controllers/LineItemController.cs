using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accounting.Data;
using Accounting.Domain;
using Accounting.Application.Models;
using Microsoft.Extensions.Logging;
using Accounting.Application.Services;
using Microsoft.AspNetCore.Http;
using System;

namespace Accounting.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineItemController : ControllerBase
    {
        private readonly ILogger<LineItemController> _logger;
        private readonly IInvoiceRepository _repository;

        public LineItemController(IInvoiceRepository context, ILogger<LineItemController> logger, IHttpContextAccessor httpContext) : base()
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

        // GET: api/LineItem
        [HttpGet("ByInvoice/{id}")]
        public async Task<ActionResult<IEnumerable<LineItemDto>>> GetLineItemsByInvoice(int id)
        {
            var invoice = await _repository.GetInvoiceById(id);

            if (invoice == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map(invoice.LineItems));
        }

        // GET: api/LineItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LineItemDto>> GetLineItem(int id)
        {
            var lineItem = await _repository.GetLineItemById(id);

            if (lineItem == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map(lineItem));
        }

        // PUT: api/LineItem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<LineItemDto>> UpdateLineItem(int id, LineItemDto lineItemDto)
        {
            if (id != lineItemDto.Id)
            {
                return BadRequest();
            }

            var lineItem = await _repository.UpdateLineItem(id, Mapper.Map(lineItemDto));
            return Ok(Mapper.Map(lineItem));
        }

        // POST: api/LineItem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LineItemDto>> CreateLineItem(LineItemDto lineItemDto)
        {
            try
            {
                // Map the new LineItem and add it.
                var lineItemId = await _repository.CreateLineItem(Mapper.Map(lineItemDto));
                return CreatedAtAction("GetLineItem", new { id = lineItemId }, lineItemDto);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }

        // DELETE: api/LineItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLineItem(int id)
        {
            await _repository.DeleteLineItem(id);
            return NoContent();
        }
    }
}
