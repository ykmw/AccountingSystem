using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public enum InvoiceStatus
    {
        Draft,
        Issued,
        Expired,
        Cancelled,
        Paid
    }
}
