using System;
using Accounting.Domain;

namespace Accounting.Data
{
    public class LineItemFactory
    {
        private string _name = string.Empty;
        private string _description = string.Empty;
        private int _quantity = default;
        private decimal _amount = default;
        private decimal _gST = default;
        private bool _isZeroRated = false;
        private int _invoiceId = default;
        public static LineItemFactory SimpleValid()
        {
            return new LineItemFactory()
            {
                _name = GenerateProperty("LineItem"),
                _description = GenerateProperty("LineItem"),
                _quantity = new Random().Next(1, 5),
                _amount = Convert.ToDecimal(new Random().Next(1, 10)),
                _gST = 0,
                _isZeroRated = false
            };
        }

        public static LineItemFactory Default() => new();

        public LineItemFactory ForThisInvoice(int invoiceId)
        {
            this._invoiceId = invoiceId;
            return this;
        }

        public LineItem Build()
        {
            var lineItem = new LineItem
            {
                Name = _name,
                Description = _description,
                Amount = _amount,
                Quantity = _quantity,
                GST = _gST,
                Total = _amount * _quantity,
                IsZeroRated = _isZeroRated,
                InvoiceId = _invoiceId
            };
            return lineItem;
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
                case "LineItem":
                    return $"{(LineItemName)new Random().Next(0, 5)}";
                case "ShortCode":
                    return $"OU{new Random().Next(10, 20)}";
                default:
                    return "Unknown property";
            }
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
