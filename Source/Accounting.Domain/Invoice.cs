using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Accounting.Domain
{
    /// <summary>
    /// The Invocie Entity.
    /// 
    /// The primary objective of NZBA Accounts is to Invoice.
    /// </summary>
    /// <remarks>
    /// After making changes to this class, you may need to add database migrations.
    /// See the <see href="https://cbanewzealand.visualstudio.com/Accounting%20Software/_wiki/wikis/Accounting-Software.wiki/38/Migrations">wiki</see>.
    /// </remarks>
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public string InvoiceRef { get; set; } = default!;
        public string PurchaseOrder { get; set; } = default!;
        public string Date { get; set; } = default!;
        public bool IsGSTExclusive { get; set; } = true; // concerns display of lineitems - e.g. dispay no GST
        public decimal SubTotal { get; set; }
        public decimal GST { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; }
        public Organisation Organisation { get; set; } = default!;
        public int OrganisationId { get; set; }
        public List<LineItem> LineItems { get; set; } = new List<LineItem>();
        public Customer? Customer { get; set; } = default;

        public static void ValidateInvoiceStatus(int currentStatus, int newStatus)
        {
            if (currentStatus == (int)InvoiceStatus.Cancelled)
            {
                throw new CustomApplicationException($"Cannot change an Invoice status once Cancelled.");
            }
            if (currentStatus == (int)InvoiceStatus.Paid)
            {
                throw new CustomApplicationException($"Cannot change an Invoice status once Paid.");
            }
            if (currentStatus == (int)InvoiceStatus.Expired)
            {
                throw new CustomApplicationException($"Cannot change an Invoice status once Expired.");
            }
            if (newStatus == (int)InvoiceStatus.Draft)
            {
                throw new CustomApplicationException($"Cannot change an Invoice status back to Draft.");
            }
        }
    }
}
