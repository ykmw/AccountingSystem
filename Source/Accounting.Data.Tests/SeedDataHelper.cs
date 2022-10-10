using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Accounting.Domain;
using Accounting.Data;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Accounting.Data.Tests
{
    public static class SeedDataHelper
    {
        public static string GenerateProperty(string property)
        {
            switch (property)
            {
                case "InvoiceRef":
                    return $"ORD-{new Random().Next(1000, 9999)}";
                case "LineItemName":
                    return $"{(LineItemName)new Random().Next(0, 5)}";
                case "CustomerName":
                    return $"{(CustomerNames)new Random().Next(0, 7)}";
                case "FirstName":
                    return $"{(FirstName)new Random().Next(0, 11)}";
                case "LastName":
                    return $"{(LastName)new Random().Next(0, 11)}";
                case "Email":
                    return $"Tester{new Random().Next(0, 1000)}@test.com";
                default:
                    return "Unknown property";
            }
        }
        public static void SeedEntity(DbContextOptions<AccountingDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions, object typeOfEntity, int numberOfEntities = 1)
        {
            using var seedContext = new AccountingDbContext(options, Options.Create<OperationalStoreOptions>(new OperationalStoreOptions()));
            var entityList = GenerateEntity(typeOfEntity, numberOfEntities);

            switch (typeOfEntity)
            {
                case Organisation:
                    {
                        foreach (var newentity in entityList.Cast<Organisation>())
                        {
                            seedContext.Organisation.Add(newentity);
                        }
                        break;
                    }
                case Customer:
                    {
                        foreach (var newentity in entityList.Cast<Customer>())
                        {
                            seedContext.Customer.Add(newentity);
                        }
                        break;
                    }
                case Invoice:
                    {
                        foreach (var newentity in entityList.Cast<Invoice>())
                        {
                            seedContext.Invoice.Add(newentity);
                        }
                        break;
                    }
                case LineItem:
                    {
                        foreach (var newentity in entityList.Cast<LineItem>())
                        {
                            seedContext.LineItem.Add(newentity);
                        }
                        break;
                    }
                case User:
                    {
                        foreach (var newentity in entityList.Cast<User>())
                        {
                            seedContext.User.Add(newentity);
                        }
                        break;
                    }
                default:
                    throw new Exception($"Cannot Generate Entity of type: {typeOfEntity.GetType().Name} type not found.");
            }

            seedContext.Organisation.TagWith($"Test: Context.{typeOfEntity.GetType().Name}.Create");
            seedContext.SaveChanges();
        }
        private static List<object> GenerateEntity(object entity, int numberOfEntities)
        {
            var entities = new List<object>();
            for (var i = 0; i < numberOfEntities; i++)
            {
                switch (entity)
                {
                    case Organisation:
                        entities.Add(OrganisationFactory.SimpleValid().Build());
                        break;
                    case Customer:
                        var customer = new Customer(OrganisationFactory.GenerateProperty("OrganisationName"))
                        {
                            ContactName = GenerateProperty("FirstName"),
                            ContactEmail = GenerateProperty("Email"),
                            Address = AddressFactory.FullAddress().Build(),
                            Phone = OrganisationFactory.GeneratePhone()
                        };
                        entities.Add(customer);
                        break;
                    case Invoice:
                        entities.Add(new Invoice
                        {
                            InvoiceRef = GenerateProperty("InvoiceRef")
                        });
                        break;
                    case LineItem:
                        entities.Add(new LineItem
                        {
                            Name = GenerateProperty("LineItemName")
                        });
                        break;
                    case User:
                        entities.Add(UserFactory.GenerateUser());
                        break;
                    default:
                        throw new Exception($"Cannot Generate Entity of type: {entity.GetType().Name} type not found.");
                }
            }
            return entities;
        }

        #region sample data
        private enum FirstName
        {
            William,
            Emily,
            ArthurConan,
            Leo,
            John,
            Sarah,
            Oscar,
            Bill,
            Charles,
            Jon,
            Mary,
            Ernest
        }
        private enum LastName
        {
            Shakespeare,
            Dickinson,
            Doyle,
            Tolstoy,
            Donne,
            Williams,
            Wilde,
            Blake,
            Dickens,
            Keats,
            Shelley,
            Hemingway
        }
        private enum LineItemName
        {
            LabourCost,
            Materials,
            ConcertTicket,
            EntryFee,
            CallOutFee,
            Delivery
        }
        private enum CustomerNames
        {
            Annie,
            Burt,
            Charles,
            Dan,
            Elle,
            Fred,
            Gale,
            Heather,
            India
        }
        #endregion
    }
}
