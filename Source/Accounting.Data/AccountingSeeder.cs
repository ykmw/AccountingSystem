using System;
using System.Collections.Generic;
using System.Linq;
using Accounting.Domain;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Accounting.Data
{
    public class AccountingSeeder
    {
        private readonly AccountingDbContext _context;

        public AccountingSeeder(AccountingDbContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            _context.Database.EnsureCreated();

            var user = _context.User.FirstOrDefault();
            if (user is null)
            {
                // Create a new User
                user = UserFactory.Create();
                user.UserName = "test@test.com";
                user.PasswordHash = "password";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;

                _context.User.Add(user);
                _context.SaveChanges();
            }

            // Register a new Organisation
            var org = OrganisationFactory.SimpleValid().Build();
            user.RegisterOrganisation(org);
            _context.SaveChanges();

            for (var i = 0; i < 10; i++)
            {
                // Add Invoice to User Organisation
                var invoice = InvoiceFactory.SimpleValid().ForThisOrganisation(org.Id).Build();
                _context.Add(invoice);
                _context.SaveChanges();

                // Add Customer to Invoice
                var customer = CustomerFactory.SimpleValid().ForThisInvoice(invoice.Id).Build();
                _context.Add(customer);
                _context.SaveChanges();

                for (var x = 0; x < 3; x++)
                {
                    // Add LineItem to Invoice
                    var lineItem = LineItemFactory.SimpleValid().ForThisInvoice(invoice.Id).Build();
                    _context.Add(lineItem);
                    _context.SaveChanges();
                }
            }
        }
    }
}
