using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.Domain;

namespace Accounting.Application.Models
{
    public class LineItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal GST { get; set; }
        public decimal Total { get; set; }
        public bool IsZeroRated { get; set; } = false;
        public int InvoiceId { get; set; }
    }
}
