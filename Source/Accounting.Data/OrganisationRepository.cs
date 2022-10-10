using System.Linq;
using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Accounting.Data
{
    /// <summary>
    /// The OrganisationRepository encapsulates data access
    /// to the User and Organisation classes.
    /// </summary>
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly AccountingDbContext _context;
        private User? _user;

        public OrganisationRepository(AccountingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Finds the first user with userName. 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Returns the user found, or a null user is none found.</returns>
        public User? FindUser(string userName)
        {
            // Lookup current User
            _user = _context.User
                .Include("_organisation")
                .FirstOrDefault(u => u.UserName.Equals(userName));
            return _user;
        }

        public async Task<Organisation> RegisterOrganisation(Organisation organisation)
        {
            _user!.RegisterOrganisation(organisation);
            await _context.SaveChangesAsync();

            return organisation;
        }

        public async Task<Organisation> GetOrganisationForUser()
        {
            // Get All Invoices include Customer and customer
            return await _context.Organisation
                .Where(i => i.Id == _user!.Organisation.Id)
                .FirstOrDefaultAsync();
        }
    }
}
