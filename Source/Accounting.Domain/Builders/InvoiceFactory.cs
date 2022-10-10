using System;
using Accounting.Domain;

namespace Accounting.Data
{
    public class InvoiceFactory
    {
        private string _date = string.Empty;
        private int _discount = default;
        private int _gst = default;
        private string _invoiceRef = string.Empty;
        private readonly bool _isGstExclusive = true;
        private int _organisationId = default!;
        private string _purchaseOrder = string.Empty;
        private int _status = default;
        private int _subtotal = default;
        private int _total = default;

        public Customer? Customer { get; set; } = null;
        public static InvoiceFactory SimpleValid()
        {
            return new InvoiceFactory()
            {
                Customer = CustomerFactory.SimpleValid().Build(),
                _date = DateTime.UtcNow.ToString(),
                _discount = 0,
                _gst = 0,
                _invoiceRef = GenerateProperty("InvoiceName"),
                _purchaseOrder = GenerateProperty("InvoiceName"),
                _status = (int)InvoiceStatus.Draft,
                _subtotal = 0,
                _total = 0
            };
        }

        public static InvoiceFactory Default() => new();

        public InvoiceFactory ForThisOrganisation(int orgId)
        {
            this._organisationId = orgId;
            return this;
        }

        public Invoice Build()
        {
            var invoice = new Invoice
            {
                Customer = Customer,
                Date = _date,
                Discount = _discount,
                GST = _gst,
                InvoiceRef = _invoiceRef,
                IsGSTExclusive = _isGstExclusive,
                OrganisationId = _organisationId,
                PurchaseOrder = _purchaseOrder,
                Status = _status,
                SubTotal = _subtotal,
                Total = _total
            };
            return invoice;
        }

        public static string GenerateProperty(string property)
        {
            switch (property)
            {
                case "InvoiceName":
                    {
                        var pt1 = ((OrgNamePt1)new Random().Next(0, 8)).ToString();
                        var pt2 = ((OrgNamePt2)new Random().Next(0, 7)).ToString();
                        var pt3 = ((OrgNamePt3)new Random().Next(0, 3)).ToString();
                        if (new Random().Next(0, 2) == 1)
                        { pt3 = ""; }

                        return $"{pt1} {pt2} {pt3}";
                    }

                case "ShortCode":
                    return $"OU{new Random().Next(10, 20)}";
                default:
                    return "Unknown property";
            }
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
    }
}
