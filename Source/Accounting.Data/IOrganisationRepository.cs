using System.Threading.Tasks;
using Accounting.Domain;

namespace Accounting.Data
{
    public interface IOrganisationRepository
    {
        User? FindUser(string userName);
        Task<Organisation> RegisterOrganisation(Organisation organisation);
        Task<Organisation> GetOrganisationForUser();
    }
}
