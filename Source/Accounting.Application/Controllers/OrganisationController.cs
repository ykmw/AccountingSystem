using System.Net.Http;
using System.Threading.Tasks;
using Accounting.Application.Models;
using Accounting.Application.Services;
using Accounting.Data;
using Accounting.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganisationController : ControllerBase
    {
        private readonly ILogger<OrganisationController> _logger;
        private readonly IOrganisationRepository _repository;
        private readonly User? _user;

        public OrganisationController(IOrganisationRepository context, ILogger<OrganisationController> logger, IHttpContextAccessor httpContext) : base()
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

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrganisationForRegistrationDto>> RegisterOrganisation([FromBody] OrganisationForRegistrationDto organisationDto)
        {
            if (_user!.IsOrganisationRegistered)
                return ValidationProblem("An organisation is already registered for the user");

            var organisation = await _repository.RegisterOrganisation(Mapper.Map(organisationDto));
            return Ok(Mapper.Map(organisation));
        }

        // GET: api/Organisation/5
        [HttpGet()]
        public async Task<ActionResult<OrganisationForRegistrationDto>> GetOrganisation()
        {
            var organisation = await _repository.GetOrganisationForUser();
            if (organisation == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map(organisation!));
        }
    }
}
