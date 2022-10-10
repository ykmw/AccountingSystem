using System.Linq;
using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Data
{
    /// <summary>
    /// The CustomerRepository encapsulates data access
    /// to the User and Customer classes.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AccountingDbContext _context;
        private User? _user;
        public User? User { get => _user; private set => _user = value; }

        public CustomerRepository(AccountingDbContext context)
        {
            _context = context;
        }
        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
        /// <summary>
        /// Finds the first user with userName. 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Returns the user found, or a null user is none found.</returns>
        public User? FindUser(string userName)
        {
            // Lookup current User
            User = _context.User
                .Include("_organisation")
                .FirstOrDefault(u => u.UserName.Equals(userName));
            return User;
        }
        public async Task<IEnumerable<Customer>> GetAllCustomersByUser()
        {
            // Get All Invoices include Customer and customer
            // Customers (with invoices) that belong to this Users Customer
            return await _context.Customer
                .Include(i => i.Invoice)
                .Where(i => i.Invoice.Organisation.Id == User!.Organisation.Id)
                .Select(i => i)
                .ToListAsync();
        }
        public async Task<Customer> GetCustomerById(int id)
        {
            // Get All Invoices include Customer and customer
            return await _context.Customer.FindAsync(id);
        }
        public async Task<Customer> UpdateCustomer(int id, Customer customerToUpdate)
        {
            // Is this customers invoice is still in draft status.
            var customer = await _context.Customer
                            .Include(i => i.Invoice)
                            .Where(c => c.Id == id)
                            .FirstOrDefaultAsync();

            // Check that User belongs to this Invoice Organisation 
            if (User!.Organisation.Id != customer.Invoice.OrganisationId)
            {
                throw new CustomApplicationException($"User cannot update an Invoice for Customer they do not belong to.");
            }
            if (customer is null)
            {
                throw new CustomApplicationException($"Customer not found. Please contact support.");
            }
            if (customer!.Invoice.Status != (int)InvoiceStatus.Draft)
            {
                throw new CustomApplicationException($"Customer update failed. Invoice must be in draft status.");
            }

            _context.ChangeTracker.Clear();

            try
            {
                // Note: If a value is not supplied it is set to it's default.
                _context.Customer.Update(customerToUpdate);
                _context.Entry(customerToUpdate).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    throw new CustomApplicationException($"Customer not found. Please contact support.");
                }
                else
                {
                    throw;
                }
            }
            return customer;
        }
        public async Task<int> CreateCustomer(Customer customerToAdd)
        {
            // Find the Invoice
            var invoice = _context.Invoice
                .Include(i => i.Customer)
                .Where(i => i.Id == customerToAdd.InvoiceId)
                .FirstOrDefault();

            // Check the Invoice exists
            if (invoice is null)
            {
                throw new CustomApplicationException($"Invoice not found. Please contact support.");
            }

            // Check that User belongs to this Invoice Organisation 
            if (User!.Organisation.Id != invoice.Organisation.Id)
            {
                throw new CustomApplicationException($"User cannot update an Invoice for Customer they do not belong to.");
            }

            // Check the Invoice is a draft
            if (invoice.Status != (int)InvoiceStatus.Draft)
            {
                throw new CustomApplicationException($"Invoice update failed. Invoice must be in draft status.");
            }

            invoice.Customer = customerToAdd;
            await _context.SaveChangesAsync();
            return customerToAdd.Id;
        }
    }
}
