using System;
using Accounting.Domain;

namespace Accounting.Data
{
    public class CustomerFactory
    {
        private string _name = string.Empty;
        private bool _isGSTExempt = false;
        private string _contactName = string.Empty;
        private string _contactEmail = string.Empty;
        private int _invoiceId = default;

        public Address Address { get; set; } = default!;
        public Phone Phone { get; set; } = default!;

        public static CustomerFactory SimpleValid()
        {
            return new CustomerFactory()
            {
                _name = GenerateProperty("CustomerName"),
                _isGSTExempt = false,
                _contactName = GenerateProperty("Name"),
                _contactEmail = GenerateProperty("Email"),
                Address = AddressFactory.FullAddress().Build(),
                Phone = GeneratePhone(),
                _invoiceId = 0
            };
        }

        public static CustomerFactory Default() => new();

        public CustomerFactory ForThisInvoice(int invoiceId)
        {
            this._invoiceId = invoiceId;
            return this;
        }

        public Customer Build()
        {
            var customer = new Customer(_name)
            {
                IsGSTExempt = _isGSTExempt,
                ContactName = _contactName,
                ContactEmail = _contactEmail,
                Address = Address,
                Phone = Phone,
                InvoiceId = _invoiceId
            };
            return customer;
        }

        public static string GenerateProperty(string property)
        {
            switch (property)
            {
                case "CustomerName":
                    {
                        var pt1 = ((OrgNamePt1)new Random().Next(0, 8)).ToString();
                        var pt2 = ((OrgNamePt2)new Random().Next(0, 7)).ToString();
                        var pt3 = ((OrgNamePt3)new Random().Next(0, 3)).ToString();
                        if (new Random().Next(0, 2) == 1)
                        { pt3 = ""; }

                        return $"{pt1} {pt2} {pt3}";
                    }

                case "PurchaseOrder":
                    return $"OU{new Random().Next(10, 20)}";
                case "Name":
                    return $"{(UserNames)new Random().Next(0, 11)}";
                case "LastName":
                    return $"{(UserLastName)new Random().Next(0, 11)}";
                case "Email":
                    return $"Tester{new Random().Next(0, 1000)}@test.com";
                default:
                    return "Unknown property";
            }
        }

        public static Phone GeneratePhone()
        {
            return new Phone("04", "7654321");
        }
        private enum OrgNamePt1
        {
            Absolute,
            Blue,
            Clever,
            Delta,
            Edge,
            First,
            Insights,
            Ocean,
        }
        private enum OrgNamePt2
        {
            Accounting,
            Consulting,
            Servcies,
            Group,
            Legal,
            Practice,
            Professionals
        }
        private enum OrgNamePt3
        {
            Ltd,
            Corp,
            Incorporated,
            co
        }
        private enum UserNames
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
        private enum UserLastName
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
    }
}
