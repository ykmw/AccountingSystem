using System;
using System.Data.Common;
using Accounting.Data;
using IdentityServer4.EntityFramework.Options;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Accounting.Application.Tests.Unit
{
    /// <summary>
    /// Provide an AccountingDbContext using Sqlite
    /// </summary>
    public class DatabasePerTest : IDisposable
    {
        private readonly DbConnection _connection;
        private bool _isDisposed;

        public DatabasePerTest()
        {
            _connection = CreateDbConnection();

            var context = CreateContext();
            context.Database.EnsureCreated();
        }

        protected AccountingDbContext CreateContext()
        {
            var dbOptions = new DbContextOptionsBuilder<AccountingDbContext>()
                   .UseSqlite(_connection)
                   .Options;

            var operationalStoreOptions = Options.Create(new OperationalStoreOptions());

            return new AccountingDbContext(dbOptions, operationalStoreOptions);
        }

        private static DbConnection CreateDbConnection()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            return connection;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _connection.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
