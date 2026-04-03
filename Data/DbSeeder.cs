using NPoco;
using SixBeeHealthCare.Web.Models;

namespace SixBeeHealthCare.Web.Data
{
    public static class DbSeeder
    {
        public static void Seed(IDatabase db)
        {
            var exists = db.ExecuteScalar<int>(
                "SELECT COUNT(1) FROM AdminUsers WHERE Email = @0",
                "admin@sixbee.nhs.uk");

            if (exists == 0)
            {
                db.Insert(new AdminUser
                {
                    Email = "admin@sixbee.nhs.uk",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin1234!")
                });
            }
        }
    }
}
