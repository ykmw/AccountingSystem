using System.Collections.Generic;
using System.Threading.Tasks;
using Accounting.Domain;

namespace Accounting.Data
{
    public interface ICustomerRepository
    {
        User? User { get; }
        User? FindUser(string userName);
        Task<IEnumerable<Customer>> GetAllCustomersByUser();
        Task<Customer> GetCustomerById(int id);
        Task<Customer> UpdateCustomer(int id, Customer customerToUpdate);
        Task<int> CreateCustomer(Customer customerToAdd);
    }
}
