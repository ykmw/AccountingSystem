namespace Accounting.Domain
{
    /// <summary>
    /// The Invoice LineItem
    /// 
    /// Invoices may contain one or many LineItems. A they are a breakdown of the costs in an invoice.
    /// </summary>
    /// <remarks>
    /// After making changes to this class, you may need to add database migrations.
    /// See the <see href="https://cbanewzealand.visualstudio.com/Accounting%20Software/_wiki/wikis/Accounting-Software.wiki/38/Migrations">wiki</see>.
    /// </remarks>
    public class LineItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal GST { get; set; }
        public decimal Total { get; set; }
        public bool IsZeroRated { get; set; } = false;
        public Invoice Invoice { get; set; } = default!;
        public int InvoiceId { get; set; }
    }
}
