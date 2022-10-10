using System;
using Microsoft.AspNetCore.Identity;

namespace Accounting.Domain
{
    /// <summary>
    /// The User Entity.
    /// 
    /// This entity Inherits from IdentityUser - the user for .NetCore.Identity. In this way we can extend .NetCore.Identity.
    /// </summary>
    /// <remarks>
    /// After making changes to this class, you may need to add database migrations.
    /// See the <see href="https://cbanewzealand.visualstudio.com/Accounting%20Software/_wiki/wikis/Accounting-Software.wiki/38/Migrations">wiki</see>.
    /// </remarks>
    public class User : IdentityUser
    {
        // Default constructor is added because EF has problems with the newly added phone entity being in the constructor.
        public User()
        {
        }

        // User must have fistname, lastname and phone 
        public User(string email, string firstName, string lastName, Phone phone) : this()
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException($"'{nameof(email)}' cannot be null or empty.", nameof(email));
            }

            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException($"'{nameof(firstName)}' cannot be null or empty.", nameof(firstName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException($"'{nameof(lastName)}' cannot be null or empty.", nameof(lastName));
            }

            UserName = email;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }

        private Organisation? _organisation = null;
        public Organisation Organisation => _organisation ?? Organisation.NotRegistered;
        public bool IsOrganisationRegistered => _organisation != null;

        [PersonalData]
        public string FirstName { get; set; } = string.Empty;

        [PersonalData]
        public string LastName { get; set; } = string.Empty;

        [PersonalData]
        public Phone Phone { get; private set; } = default!;

        public void RegisterOrganisation(Organisation organisation)
        {
            if (IsOrganisationRegistered)
            {
                throw new InvalidOperationException("An organisation is already registered");
            }

            _organisation = organisation;
        }
    }
}
