using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Accounting.Domain
{
    /// <summary>
    /// The Phone Entity.
    /// 
    /// Used to keep logic relating to phone numbers and prefixes isolated from domain objects. 
    /// </summary>
    /// <remarks>
    /// After making changes to this class, you may need to add database migrations.
    /// See the <see href="https://cbanewzealand.visualstudio.com/Accounting%20Software/_wiki/wikis/Accounting-Software.wiki/38/Migrations">wiki</see>.
    /// </remarks>
    public class Phone
    {
        private string _phoneNumberPrefix = default!;
        private string _phoneNumber = default!;

        /// <summary>
        /// Constructor for the type Phone
        /// 
        /// New Zealand landline phone numbers have a total of eight digits, excluding the leading 0: 
        /// a one-digit area code, and a seven-digit phone number (e.g. 09 700 1234), 
        /// beginning with a digit between 2 and 9 (but excluding 900, 911, and 999 due to misdial guards). 
        /// There are five regional area codes: 3, 4, 6, 7, and 9.
        /// 
        /// Telephone numbers for mobile phones begin with 02, followed by seven to nine digits (usually eight). 
        /// The first few digits after the 02 indicate the original mobile network that issued the number.
        /// The longest valid mobile variant is operater TAlk2 - 4 digit prefix and 7 digit number 
        /// 
        /// https://en.wikipedia.org/wiki/Telephone_numbers_in_New_Zealand
        /// </summary>
        /// <param name="phoneNumberPrefix"></param>
        /// <param name="phoneNumber"></param>
        public Phone(string phoneNumberPrefix, string phoneNumber)
        {
            PhoneNumberPrefix = phoneNumberPrefix;
            PhoneNumber = phoneNumber;
        }

        /// <summary>
        /// A phone prefix - 1-4 digits are valid 
        /// </summary>
        public string PhoneNumberPrefix
        {
            get => _phoneNumberPrefix; set
            {
                if (value.ToString().Length < 1)
                {
                    throw new CustomApplicationException($"Minimum PhoneNumberPrefix length is 1 digit.");
                }
                else if (value.ToString().Length > 4)
                {
                    throw new CustomApplicationException($"Maximum Phone Number Prefix length is 4 digits.");
                }
                _phoneNumberPrefix = value;
            }
        }

        /// <summary>
        /// A phone number - 4-9 digits are valid 
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber; set
            {
                if (value.ToString().Length < 4)
                {
                    throw new CustomApplicationException($"Minimum Phone Number length is 4 digits.");
                }
                else if (value.ToString().Length > 9)
                {
                    throw new CustomApplicationException($"Maximum Phone Number length is 9 digits.");
                }
                _phoneNumber = value;
            }
        }
    }
}
