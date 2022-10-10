using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.Domain;
using Newtonsoft.Json.Linq;

namespace Accounting.Application.Models
{
    /// <summary>
    /// The Phone.
    /// </summary>
    public class PhoneDto
    {
        private string _phoneNumberPrefix = default!;
        private string _phoneNumber = default!;

        /// <summary>
        /// Constructor for the type Phone
        /// </summary>
        /// <param name="phoneNumberPrefix"></param>
        /// <param name="phoneNumber"></param>
        public PhoneDto(string phoneNumberPrefix, string phoneNumber)
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
                    throw new InvalidRangeException($"Minimum PhoneNumberPrefix length is 1 digit.");
                }
                else if (value.ToString().Length > 4)
                {
                    throw new InvalidRangeException("Maximum Phone Number Prefix length is 4 digits.");
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
                    throw new InvalidRangeException("Minimum Phone Number length is 4 digits.");
                }
                else if (value.ToString().Length > 9)
                {
                    throw new InvalidRangeException("Maximum Phone Number length is 9 digits.");
                }
                _phoneNumber = value;
            }
        }
    }
}
