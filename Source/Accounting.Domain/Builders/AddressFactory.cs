using System;
using System.ComponentModel.DataAnnotations;
using Accounting.Domain;

namespace Accounting.Data
{
    public class AddressFactory
    {
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string AddressLine3 { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;

        public static Address Empty()
        {
            return new Address(
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty);
        }
        public static AddressFactory FullAddress()
        {
            var addressFactory = new AddressFactory
            {
                AddressLine1 = GenerateProperty("Line1"),
                AddressLine2 = GenerateProperty("Line2"),
                AddressLine3 = GenerateProperty("Line3"),
                PostCode = GenerateProperty("PostCode")
            };
            return addressFactory;
        }
        public static AddressFactory No_Line2()
        {
            var addressFactory = new AddressFactory
            {
                AddressLine1 = GenerateProperty("Line1"),
                AddressLine2 = string.Empty,
                AddressLine3 = GenerateProperty("Line3"),
                PostCode = GenerateProperty("PostCode")
            };
            return addressFactory;
        }
        public static AddressFactory No_PostCode()
        {
            var addressFactory = new AddressFactory
            {
                AddressLine1 = GenerateProperty("Line1"),
                AddressLine2 = GenerateProperty("Line2"),
                AddressLine3 = GenerateProperty("Line3"),
                PostCode = string.Empty
            };
            return addressFactory;
        }
        public Address Build()
        {
            return new Address(AddressLine1, AddressLine2, AddressLine3, PostCode);
        }
        private static string GenerateProperty(string property)
        {
            switch (property)
            {
                case "Line1":
                    return $"{new Random().Next(0, 300)} {(Line1a)new Random().Next(0, 5)} {(Line1b)new Random().Next(0, 5)}";
                case "Line2":
                    return $"{(Line2)new Random().Next(0, 4)}";
                case "Line3":
                    return $"{(Line3)new Random().Next(0, 5)}";
                case "PostCode":
                    return $"{new Random().Next(1000, 9900)}";
                default:
                    return "Unknown property";
            }
        }
        private enum Line1a
        {
            Bank,
            High,
            Old,
            Brick,
            Nui
        }
        private enum Line1b
        {
            Road,
            Street,
            Lane,
            Hill,
            Park
        }
        private enum Line2
        {
            Bank,
            Park,
            House,
            Shire
        }
        private enum Line3
        {
            Auckland,
            SouthIsland,
            Northland,
            Wellington,
            Dunedin
        }
    }
}
