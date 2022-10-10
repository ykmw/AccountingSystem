using Accounting.Data;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Application.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly AccountingDbContext _context;

        public BaseController(AccountingDbContext context)
        {
            _context = context;
        }

        protected new IActionResult Ok()
        {
            _context.SaveChanges();
            return base.Ok();
        }

        protected new IActionResult NoContent()
        {
            _context.SaveChanges();
            return base.NoContent();
        }
    }
}
