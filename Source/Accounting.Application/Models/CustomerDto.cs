using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Accounting.Application.Models
{
    public class CustomerDto
    {
        public CustomerDto()
        {

        }
        public CustomerDto(string name) : base()
        {
            Name = name;
        }

        public int Id { get; set; }
        [PersonalData]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;
        public bool IsGSTExempt { get; set; } = false;
        [PersonalData]
        [MaxLength(250)]
        public string ContactName { get; set; } = default!;
        [PersonalData]
        [MaxLength(250)]
        public string ContactEmail { get; set; } = default!;
        [PersonalData]
        public AddressDto Address { get; set; } = default!;
        [PersonalData]
        public PhoneDto Phone { get; set; } = default!;
        public int InvoiceId { get; set; }
    }
}
