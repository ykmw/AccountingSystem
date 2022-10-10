using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Accounting.Domain
{
    /// <summary>
    /// The Customer Entity.
    /// 
    /// Each Invocie is issued to a customer.
    /// </summary>
    /// <remarks>
    /// After making changes to this class, you may need to add database migrations.
    /// See the <see href="https://cbanewzealand.visualstudio.com/Accounting%20Software/_wiki/wikis/Accounting-Software.wiki/38/Migrations">wiki</see>.
    /// </remarks>
    public class Customer
    {
        public Customer(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        [PersonalData]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public bool IsGSTExempt { get; set; } = false;
        [PersonalData]
        [MaxLength(250)]
        public string ContactName { get; set; } = default!;
        [PersonalData]
        [MaxLength(250)]
        public string ContactEmail { get; set; } = default!;
        [PersonalData]
        public Address Address { get; set; } = default!;
        [PersonalData]
        public Phone Phone { get; set; } = default!;
        public int InvoiceId { get; set; } // Note that InvoiceId may make it impossible to create a customer without first creating an invoice.
        public Invoice Invoice { get; set; } = default!;
    }
}
