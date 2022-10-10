using System;
using Accounting.Data;
using Xunit;

namespace Accounting.Domain.Tests
{
    public class OrganisationTests
    {
        [Fact]
        public void Street1IsRequired()
        {
            Assert.Throws<ArgumentException>(() => new Address(string.Empty, string.Empty, "citytown", string.Empty));
        }

        [Fact]
        public void CityTownIsRequired()
        {
            Assert.Throws<ArgumentException>(() => new Address("street1", string.Empty, string.Empty));
        }

        public static readonly User NewUser = new("test@test.com", "First Name", "Last Name", new Phone("01", "2345678"));

        [Fact]
        public void DefaultOrganisationIsUnregistered()
        {
            var user = NewUser;

            Assert.Same(user.Organisation, Organisation.NotRegistered);
            Assert.False(user.IsOrganisationRegistered);
        }

        [Fact]
        public void CannotReregisterOrganisation()
        {
            var user = UserFactory.Create();
            var firstOrganisation = OrganisationFactory.SimpleValid().Build();
            var secondOrganisation = OrganisationFactory.SimpleValid().Build();

            user.RegisterOrganisation(firstOrganisation);

            Assert.Throws<InvalidOperationException>(() => user.RegisterOrganisation(secondOrganisation));
        }

        [Fact]
        public void NameIsRequired()
        {
            var organisation = OrganisationFactory.SimpleValid().Build();
            Assert.Throws<CustomApplicationException>(() => organisation.Name = string.Empty);
        }

        [Theory]
        [InlineData("")]
        [InlineData("xx3")]
        [InlineData("xxxx5")]
        public void ShortCodeMustBe4Characters(string shortCode)
        {
            var organisation = OrganisationFactory.SimpleValid()
                .WithShortCode(shortCode);

            Assert.Throws<ArgumentException>(() => organisation.Build());
        }
    }
}
