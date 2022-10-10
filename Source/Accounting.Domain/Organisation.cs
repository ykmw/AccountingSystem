using System;
using static AutoMapper.Internal.ExpressionFactory;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using IdentityServer4.Models;

namespace Accounting.Domain
{
    /// <summary>
    /// The Organisation Entity.
    /// 
    /// The parent record of all data stored in the Accouting solution. Each organisations data must be kept separate. 
    /// </summary>
    /// <remarks>
    /// After making changes to this class, you may need to add database migrations.
    /// See the <see href="https://cbanewzealand.visualstudio.com/Accounting%20Software/_wiki/wikis/Accounting-Software.wiki/38/Migrations">wiki</see>.
    /// 
    /// The private constructor and the use of the null-forgiving operator (!) to initialize the Address 
    /// are required for Entity Framework Core.
    /// </remarks>
    public class Organisation
    {
        /// <summary>
        /// This is used as a placeholder for new users that have not yet registered
        /// their organisation.
        /// </summary>
        public static readonly Organisation NotRegistered = new("Not registered", "NOTR", new Address("Street1", "Street2", "CityTown", "1001"), new Phone("02", "123456789"));
        private string? _gSTNumber = null;
        private string _shortCode = default!;
        private string _name = default!;
        private Organisation(string name, string shortCode)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"Organisation '{nameof(name)}' cannot be empty.", nameof(name));
            if (shortCode.Length != 4) throw new ArgumentException($"Organisation '{nameof(shortCode)}' should be 4 characters", nameof(shortCode));

            Name = name.Trim();
            ShortCode = shortCode;
        }

        // Organisation must have a phone AND address
        public Organisation(string name, string shortCode, Address address, Phone phone) : this(name, shortCode)
        {
            Address = address;
            Phone = phone;
        }

        public int Id { get; private set; }
        [Required]
        public string Name
        {
            get => _name; set
            {
                if (value.ToString().Length < 1)
                {
                    throw new CustomApplicationException($"Organisation Name is a required field and cannot be empty.");
                }
                _name = value;
            }
        }

        /// <summary>
        /// The short code is a four character representation of the organisation that 
        /// is used to generate invoice numbers.
        /// </summary>
        public string ShortCode
        {
            get => _shortCode; set
            {
                if (value.ToString().Length < 4)
                {
                    throw new CustomApplicationException($"Minimum ShortCode length is 4 digits.");
                }
                else if (value.ToString().Length > 4)
                {
                    throw new CustomApplicationException($"Minimum ShortCode length is 4 digits.");
                }
                _shortCode = value;
            }
        }

        public Address Address { get; private set; } = default!;
        public Phone Phone { get; private set; } = default!;

        public void SetOrganisationAddress(Address address)
        {
            if (this.Address is not null)
            {
                throw new CustomApplicationException($"An address already exists for this organisation.");
            }

            Address = address;
        }


        /// <summary>
        /// Whether you're a sole trader, contractor, in partnership or a company, as soon as you think you’ll earn more than $60,000 in 12 months, 
        /// you must register for GST. You may be charged penalties if you don't register when you need to.
        /// If you don't think you'll turn over that much, it's up to you whether or not to register. 
        /// One benefit of voluntary registration is you might be able to claim a GST refund, eg if you have a lot of expenses but not much income. 
        /// Once you've registered, you have to complete regular GST returns.
        /// </summary>
        public bool IsGSTRegistered()
        {
            return !string.IsNullOrEmpty(GSTNumber);
        }

        /// <summary>
        /// Goods and services tax (GST) is added to the price of most products and services. 
        /// If you’re GST registered, you can claim back the GST you pay on goods or services you buy for your business. 
        /// You can also charge GST (15%) on what you sell — this is collecting it on the government’s behalf.
        /// </summary>
        public string? GSTNumber
        {
            get => _gSTNumber; set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 8)
                {
                    throw new CustomApplicationException($"GST Number must be 8 - 9 digits long.");
                }
                else if (value.Length > 9)
                {
                    throw new CustomApplicationException($"GST Number must be 8 - 9 digits long.");
                }
                _gSTNumber = value;
            }
        }
    }
}
