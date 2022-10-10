using Accounting.Domain;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Accounting.Data
{
    public class AccountingDbContext : ApiAuthorizationDbContext<User>
    {
        public AccountingDbContext(DbContextOptions<AccountingDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        public virtual DbSet<User> User { get; set; } = default!;
        public virtual DbSet<Organisation> Organisation { get; set; } = default!;
        public virtual DbSet<Invoice> Invoice { get; set; } = default!;
        public virtual DbSet<Customer> Customer { get; set; } = default!;
        public virtual DbSet<LineItem> LineItem { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Organisation>().OwnsOne(p => p.Address);
            builder.Entity<Organisation>().OwnsOne(p => p.Phone);

            builder.Entity<User>(user =>
            {
                user.HasOne<Organisation>("_organisation")
                    .WithMany()
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<User>().OwnsOne(p => p.Phone);

            builder.Entity<Customer>().OwnsOne(p => p.Address);
            builder.Entity<Customer>().OwnsOne(p => p.Phone);
        }
    }
}
