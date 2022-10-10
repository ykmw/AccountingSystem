using System;
using System.Linq;
using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Accounting.Data
{
    /// <summary>
    /// The InvoiceRepository encapsulates data access
    /// to the Invocie and LineItem classes.
    /// </summary>
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AccountingDbContext _context;
        private User? _user;
        public User? User { get => _user; private set => _user = value; }
        public InvoiceRepository(AccountingDbContext context)
        {
            _context = context;
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.Id == id);
        }
        private bool LineItemExists(int id)
        {
            return _context.LineItem.Any(e => e.Id == id);
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
        public async Task<IEnumerable<Invoice>> GetAllInvociesByOrgId(int orgId)
        {
            // Get All Invoices that belong to this Users organisation
            return await _context.Invoice
                .Where(i => i.Organisation.Id == User!.Organisation.Id)
                .Select(i => i)
                .ToListAsync();
        }
        public async Task<Invoice> GetInvoiceById(int id)
        {
            // Get All Invoices by Id
            return await _context.Invoice
                .Include(i => i.LineItems)
                .Where(i => i.OrganisationId == User!.Organisation.Id)
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Invoice> UpdateInvoiceStatus(int id, int status)
        {
            // Get the Invoice
            var invoice = _context.Invoice
                .Where(c => c.Id == id)
                .FirstOrDefault();

            // Check the invoice exists
            if (invoice is null)
            {
                throw new CustomApplicationException($"Invoice not found. Please contact support.");
            }

            try
            {
                // Validate Invoice Status
                Invoice.ValidateInvoiceStatus(invoice.Status, status);

                // Set the new Invoice status
                invoice.Status = status;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    throw new CustomApplicationException($"Invoice not found. Please contact support.");
                }
                else
                {
                    throw;
                }
            }
            return invoice;
        }
        public async Task<Invoice> UpdateInvoice(int id, Invoice invoiceToUpdate)
        {
            // Find the Invoice
            var invoice = _context.Invoice
                .Where(c => c.Id == id)
                .FirstOrDefault();

            // Check the invoice exists
            if (invoice is null)
            {
                throw new CustomApplicationException($"Invoice not found. Please contact support.");
            }

            // Check the invoice is a draft
            if (invoice.Status != (int)InvoiceStatus.Draft)
            {
                throw new CustomApplicationException($"Update failed. Invoices may only be updated in draft status.");
            }

            _context.ChangeTracker.Clear();

            try
            {
                // Note: If a value is not supplied it is set to it's default.
                _context.Invoice.Update(invoiceToUpdate);
                _context.Entry(invoiceToUpdate).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    throw new CustomApplicationException($"Invoice not found. Please contact support.");
                }
                else
                {
                    throw;
                }
            }
            return invoice;
        }
        public async Task<int> CreateInvoice(Invoice invoiceToAdd)
        {
            // Map the invoice and add it to the Organisation
            invoiceToAdd.Organisation = User!.Organisation;
            invoiceToAdd.Status = (int)InvoiceStatus.Draft;

            _context.Invoice.Add(invoiceToAdd);
            await _context.SaveChangesAsync();

            return invoiceToAdd.Id;
        }
        public async Task<LineItem> GetLineItemById(int id)
        {
            // Get All Invoices by Id
            return await _context.LineItem.FindAsync(id);
        }
        public async Task<IEnumerable<LineItem>> GetAllLineItemsByInvoiceId(int invoiceId)
        {
            // Get All LineItems that belong to this Invoice
            return await _context.LineItem
                .Where(i => i.InvoiceId == invoiceId)
                .Select(i => i)
                .ToListAsync();
        }
        public async Task<LineItem> UpdateLineItem(int id, LineItem lineItemToUpdate)
        {
            // Find the LineItem
            var invoice = await _context.Invoice
                .Where(i => i.Id == lineItemToUpdate.InvoiceId)
                .Where(i => i.OrganisationId == User!.Organisation.Id)
                .FirstOrDefaultAsync();

            // Check the invoice exists
            if (invoice == null)
            {
                throw new CustomApplicationException($"LineItem Invoice not found. Please contact support.");
            }

            // Check Invoice is in Draft status
            if (invoice.Status != (int)InvoiceStatus.Draft)
            {
                throw new CustomApplicationException($"Update failed. Invoices may only be updated in draft status.");
            }

            _context.ChangeTracker.Clear();

            try
            {
                // Note: If a value is not supplied it is set to it's default.
                _context.LineItem.Update(lineItemToUpdate);
                _context.Entry(lineItemToUpdate).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineItemExists(id))
                {
                    throw new CustomApplicationException($"LineItem not found. Please contact support.");
                }
                else
                {
                    throw;
                }
            }
            return lineItemToUpdate;
        }
        public async Task<int> CreateLineItem(LineItem lineItemToAdd)
        {
            // Get Invoice that belongs to this Users organisation
            var invoice = await _context.Invoice
                .Where(i => i.Id == lineItemToAdd.InvoiceId)
                .Where(i => i.OrganisationId == User!.Organisation.Id)
                .FirstOrDefaultAsync();

            if (invoice == null)
            {
                throw new CustomApplicationException($"Invoice not found. Please contact support.");
            }

            // Check Invoice is in Draft status
            if (invoice.Status != (int)InvoiceStatus.Draft)
            {
                throw new CustomApplicationException($"Update failed. Invoices may only be updated in draft status.");
            }

            invoice.LineItems.Add(lineItemToAdd);
            await _context.SaveChangesAsync();
            return lineItemToAdd.Id;
        }
        public async Task<int> DeleteLineItem(int lineItemId)
        {
            // Get Invoices that belong to this Users organisation
            var invoice = await _context.Invoice
                .Include(i => i.LineItems.Where(li => li.Id == lineItemId))
                .Where(i => i.OrganisationId == User!.Organisation.Id)
                .FirstOrDefaultAsync();

            // Check LineItem exists
            if (invoice!.LineItems == null && invoice!.LineItems!.Count == 1)
            {
                throw new CustomApplicationException($"LineItem not found. Please contact support.");
            }

            // Check Invoice is in Draft status
            if (invoice.Status != (int)InvoiceStatus.Draft)
            {
                throw new CustomApplicationException($"Update failed. Invoices may only be updated in draft status.");
            }

            // Get the lineItem
            var lineitem = await _context.LineItem
                .Where(li => li.Id == lineItemId)
                .FirstOrDefaultAsync();

            // Remove the lineItem
            _context.LineItem.Remove(lineitem);
            await _context.SaveChangesAsync();
            return lineItemId;
        }
    }
}
