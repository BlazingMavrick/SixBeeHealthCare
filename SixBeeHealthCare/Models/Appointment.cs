using NPoco;

namespace SixBeeHealthCare.Web.Models
{
    [TableName("Appointments")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class Appointment
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime AppointmentDateTime { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
