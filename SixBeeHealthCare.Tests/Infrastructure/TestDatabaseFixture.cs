using Microsoft.Data.SqlClient;
using NPoco;

namespace SixBeeHealthCare.Tests.Infrastructure
{
    public sealed class TestDatabaseFixture : IDisposable
    {
        public string DatabaseName { get; } = $"SixBeeHealthCare_Tests_{Guid.NewGuid():N}";

        private static readonly string MasterConnectionString =
            Environment.GetEnvironmentVariable("SIXBEE_TEST_CONNECTION_STRING")
            ?? "Server=localhost,1433;Database=master;User Id=sa;Password=SixBee_Dev_2026!;TrustServerCertificate=True";

        public string TestConnectionString =>
            MasterConnectionString.Replace("Database=master", $"Database={DatabaseName}");

        public TestDatabaseFixture()
        {
            CreateDatabase();
            CreateSchema();
        }

        public IDatabase OpenDatabase() =>
     new Database(
         TestConnectionString,
         DatabaseType.SqlServer2012,
         SqlClientFactory.Instance);

        private IDatabase OpenMaster() =>
            new Database(
                MasterConnectionString,
                DatabaseType.SqlServer2012,
                SqlClientFactory.Instance);

        private void CreateDatabase()
        {
            using var db = OpenMaster();
            db.Execute($"CREATE DATABASE [{DatabaseName}]");
        }

        private void CreateSchema()
        {
            using var db = OpenDatabase();

            db.Execute(@"
            CREATE TABLE Appointments (
                Id                  INT             IDENTITY(1,1) PRIMARY KEY,
                PatientName         NVARCHAR(200)   NOT NULL,
                AppointmentDateTime DATETIME2       NOT NULL,
                Description         NVARCHAR(1000)  NOT NULL,
                ContactNumber       NVARCHAR(20)    NOT NULL,
                EmailAddress        NVARCHAR(254)   NOT NULL,
                IsApproved          BIT             NOT NULL DEFAULT 0,
                CreatedAt           DATETIME2       NOT NULL DEFAULT GETUTCDATE()
            )");

            db.Execute(@"
            CREATE TABLE AdminUsers (
                Id           INT           IDENTITY(1,1) PRIMARY KEY,
                Email        NVARCHAR(254) NOT NULL,
                PasswordHash NVARCHAR(MAX) NOT NULL,
                CONSTRAINT UQ_AdminUsers_Email UNIQUE (Email)
            )");
        }

        public void Dispose()
        {
            using var db = OpenMaster();
            db.Execute($"ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            db.Execute($"DROP DATABASE [{DatabaseName}]");
        }
    }

}


