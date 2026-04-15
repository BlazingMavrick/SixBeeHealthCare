using NPoco;

namespace SixBeeHealthCare.Web.Data
{
    public static class SchemaInitialiser
    {
        public static void Initialise(IDatabase db)
        {
            db.Execute(@"
            IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Appointments')
            BEGIN
                CREATE TABLE Appointments (
                    Id                  INT             IDENTITY(1,1) PRIMARY KEY,
                    PatientName         NVARCHAR(200)   NOT NULL,
                    AppointmentDateTime DATETIME2       NOT NULL,
                    Description         NVARCHAR(1000)  NOT NULL,
                    ContactNumber       NVARCHAR(20)    NOT NULL,
                    EmailAddress        NVARCHAR(254)   NOT NULL,
                    IsApproved          BIT             NOT NULL DEFAULT 0,
                    CreatedAt           DATETIME2       NOT NULL DEFAULT GETUTCDATE()
                )
            END");

            db.Execute(@"
            IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AdminUsers')
            BEGIN
                CREATE TABLE AdminUsers (
                    Id           INT          IDENTITY(1,1) PRIMARY KEY,
                    Email        NVARCHAR(254) NOT NULL,
                    PasswordHash NVARCHAR(MAX) NOT NULL,
                    CONSTRAINT UQ_AdminUsers_Email UNIQUE (Email)
                )
            END");
        }
    }
}
