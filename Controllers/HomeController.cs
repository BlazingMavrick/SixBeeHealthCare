using Microsoft.AspNetCore.Mvc;
using SixBeeHealthCare.Web.Models;
using SixBeeHealthCare.Web.Services.Interfaces;
using SixBeeHealthCare.Web.ViewModels;

namespace SixBeeHealthCare.Web.Controllers
{
    /// <summary>
    /// Handles the patient appointment booking form.
    /// </summary>
    public class HomeController(IAppointmentService appointmentService) : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(new BookingViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(BookingViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var appointment = new Appointment
            {
                PatientName = model.PatientName,
                AppointmentDateTime = model.AppointmentDateTime,
                Description = model.Description,
                ContactNumber = model.ContactNumber,
                EmailAddress = model.EmailAddress
            };

            await appointmentService.CreateAsync(appointment);

            return RedirectToAction(nameof(Confirmation));
        }

        [HttpGet]
        public IActionResult Confirmation() => View();
    }
}
