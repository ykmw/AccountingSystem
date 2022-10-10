using System;
using Accounting.Domain;

namespace Accounting.Data
{
    public class OrganisationFactory
    {
        private string _name = string.Empty;
        private string _shortCode = string.Empty;

        public Address Address { get; set; } = default!;

        public static OrganisationFactory SimpleValid()
        {
            return new OrganisationFactory()
            {
                _name = GenerateProperty("OrganisationName"),
                _shortCode = GenerateProperty("ShortCode"),
                Address = AddressFactory.FullAddress().Build()
            };
        }

        public static OrganisationFactory Default() => new();

        public OrganisationFactory WithShortCode(string shortCode)
        {
            this._shortCode = shortCode;
            return this;
        }

        public OrganisationFactory WithThisName(string name)
        {
            this._name = name;
            this._shortCode = GenerateProperty("ShortCode");
            return this;
        }

        public Organisation Build()
        {
            return new Organisation(_name, _shortCode, Address, GeneratePhone());
        }

        public static string GenerateProperty(string property)
        {
            switch (property)
            {
                case "OrganisationName":
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
    }
}
