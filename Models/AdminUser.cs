using NPoco;

namespace SixBeeHealthCare.Web.Models
{
    [TableName("AdminUsers")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class AdminUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
