using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Accounting.Domain;

namespace Accounting.Application.Models
{
    /// <summary>
    /// The Invocie Entity.
    /// 
    /// The primary objective of NZBA Accounts is to Invoice.
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceRef { get; set; } = default!;
        public string PurchaseOrder { get; set; } = default!;
        public string Date { get; set; } = default!;
        public bool IsGSTExclusive { get; set; } = true; // concerns display of lineitems - e.g. dispay no GST
        public decimal SubTotal { get; set; }
        public decimal GST { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; } // implement an enum and change datatype to small
        public List<LineItemDto> LineItem { get; set; } = new List<LineItemDto>();
    }
}
