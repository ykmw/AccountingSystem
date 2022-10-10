using System.ComponentModel.DataAnnotations;

namespace Accounting.Application.Models
{
    public class OrganisationForRegistrationDto
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(maximumLength: 4, MinimumLength = 4, ErrorMessage = "The ShortCode must be exactly 4 characters")]
        public string ShortCode { get; set; } = string.Empty;
        public string GSTNumber { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        public string AddressStreet1 { get; set; } = string.Empty;
        public string AddressStreet2 { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        public string AddressCityTown { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;
        public string PhonePrefix { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
