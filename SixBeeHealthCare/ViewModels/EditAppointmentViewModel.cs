using System.ComponentModel.DataAnnotations;

namespace SixBeeHealthCare.Web.ViewModels
{
    public class EditAppointmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200)]
        [Display(Name = "Full Name")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Appointment date and time is required")]
        [Display(Name = "Appointment Date & Time")]
        public DateTime AppointmentDateTime { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(1000)]
        [Display(Name = "Description / Reason for Visit")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
