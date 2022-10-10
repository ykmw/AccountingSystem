using System;
using System.ComponentModel.DataAnnotations;

namespace Accounting.Domain
{
    /// <summary>
    /// The Address Entity.
    /// 
    /// A physical address for the Organisation - used when creating the Invoice. 
    /// https://www.nzpost.co.nz/business/shipping-in-nz/addressing-standards
    /// </summary>
    /// <remarks>
    /// After making changes to this class, you may need to add database migrations.
    /// See the <see href="https://cbanewzealand.visualstudio.com/Accounting%20Software/_wiki/wikis/Accounting-Software.wiki/38/Migrations">wiki</see>.
    /// 
    /// The private constructor and the use of the null-forgiving operator (!) to initialize the Address 
    /// are required for Entity Framework Core.
    /// </remarks>
    public class Address
    {
        // Address line 1 AND 3
        public Address(string addressLine1, string addressLine3) : this(addressLine1, string.Empty, addressLine3)
        { }
        // Address line 1 2 AND 3
        public Address(string addressLine1, string addressLine2, string addressLine3) : this(addressLine1, addressLine2, addressLine3, string.Empty)
        { }
        // Address line 1 2 3 AND Postcode
        public Address(string addressLine1, string addressLine2, string addressLine3, string postCode) : this(addressLine1, addressLine2, addressLine3, postCode, string.Empty)
        { }

        // Address line 1 2 3 Postcode AND Country
        public Address(string addressLine1, string addressLine2, string addressLine3, string postCode, string country)
        {
            if (string.IsNullOrWhiteSpace(addressLine1)) throw new ArgumentException($"'{nameof(addressLine1)}' cannot be empty.", nameof(addressLine1));
            if (string.IsNullOrWhiteSpace(addressLine3)) throw new ArgumentException($"'{nameof(addressLine3)}' cannot be empty.", nameof(addressLine3));

            AddressLine1 = addressLine1.Trim();
            AddressLine2 = addressLine2.Trim();
            AddressLine3 = addressLine3.Trim();
            Country = country.Trim();
            PostCode = postCode.Trim();

            if (postCode.Length > 6) throw new InvalidRangeException($"Address Poscode cannot be longer than 6 characters.");
        }

        [MaxLength(250)]
        public string AddressLine1 { get; private set; }
        [MaxLength(250)]
        public string AddressLine2 { get; private set; }
        [MaxLength(250)]
        public string AddressLine3 { get; private set; }
        [MaxLength(250)]
        public string Country { get; private set; }

        /// <summary>
        /// Postcode consist of four digits, the first two of which specify the area, 
        /// the third the type of delivery (street, PO Box, Private Bag, or Rural delivery), 
        /// and the last the specific lobby, RD (rural delivery) number, or suburb. 
        /// except Freepost has 6 digits.
        /// </summary>
        [MaxLength(6)]
        public string PostCode { get; private set; }
    }
}
