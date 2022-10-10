using System.Collections.Generic;
using System.Threading.Tasks;
using Accounting.Domain;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Data
{
    public interface IInvoiceRepository
    {
        User? User { get; }
        User? FindUser(string userName);
        Task<IEnumerable<Invoice>> GetAllInvociesByOrgId(int orgId);
        Task<Invoice> GetInvoiceById(int id);
        Task<Invoice> UpdateInvoiceStatus(int id, int status);
        Task<Invoice> UpdateInvoice(int id, Invoice invoiceToUpdate);
        Task<int> CreateInvoice(Invoice invoiceToAdd);
        Task<IEnumerable<LineItem>> GetAllLineItemsByInvoiceId(int invoiceId);
        Task<LineItem> GetLineItemById(int id);
        Task<LineItem> UpdateLineItem(int id, LineItem lineItemToUpdate);
        Task<int> CreateLineItem(LineItem lineItemToAdd);
        Task<int> DeleteLineItem(int lineItemId);
    }
}
