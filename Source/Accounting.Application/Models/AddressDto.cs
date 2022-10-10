using System;
using System.ComponentModel.DataAnnotations;

namespace Accounting.Application.Models
{
    /// <summary>
    /// The Address Dto.
    /// 
    /// A physical address for the Organisation/Customer - used when creating the Invoice. 
    /// https://www.nzpost.co.nz/business/shipping-in-nz/addressing-standards
    /// </summary>
    public class AddressDto
    {
        [MaxLength(250)]
        public string AddressLine1 { get; set; } = default!;
        [MaxLength(250)]
        public string AddressLine2 { get; set; } = string.Empty;
        [MaxLength(250)]
        public string AddressLine3 { get; set; } = default!;
        [MaxLength(250)]
        public string Country { get; set; } = string.Empty;
        [MaxLength(6)]
        public string PostCode { get; set; } = string.Empty;
    }
}
