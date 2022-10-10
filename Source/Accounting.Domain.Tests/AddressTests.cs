using System;
using Accounting.Data;
using Xunit;

namespace Accounting.Domain.Tests
{
    public class AddressTests
    {
        [Theory]
        [InlineData("Rotville")]
        public void Line1IsRequired(string testAddress)
        {
            Assert.Throws<ArgumentException>(() => new Address(string.Empty, string.Empty, testAddress));
        }

        [Theory]
        [InlineData("Crook Street")]
        public void Line3IsRequired(string testAddress)
        {
            Assert.Throws<ArgumentException>(() => new Address(testAddress, string.Empty, string.Empty));
        }

        [Theory]
        [InlineData("xxxxxx7")]
        public void PostCodeMustBe6Characters(string postCode)
        {
            var addressFactory = AddressFactory.No_PostCode();
            addressFactory.PostCode = postCode;
            Assert.Throws<InvalidRangeException>(() => addressFactory.Build());
        }

        [Fact]
        public void CanRegisterAddress()
        {
            var organisationWithNoAddress = new OrganisationFactory().WithThisName("Fancy Pants Company").Build();
            organisationWithNoAddress.SetOrganisationAddress(AddressFactory.FullAddress().Build());
            Assert.NotNull(organisationWithNoAddress.Address);
        }

        [Theory]
        [InlineData("Crook Street")]
        public void CannotReregisterAddress(string testAddress)
        {
            // create a new organisation with an address
            var organisationWithAddress = OrganisationFactory.SimpleValid().Build();

            // create a new address
            var addressFactory = AddressFactory.FullAddress();
            addressFactory.AddressLine1 = testAddress;

            // try to replace the address ! should fail
            Assert.Throws<CustomApplicationException>(() => organisationWithAddress.SetOrganisationAddress(addressFactory.Build()));
        }
    }
}
