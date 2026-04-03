using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixBeeHealthCare.Web.Services.Interfaces;
using SixBeeHealthCare.Web.ViewModels;

namespace SixBeeHealthCare.Web.Controllers
{
    /// <summary>
    /// Appointment management: list, edit, approve, delete.
    /// </summary>
    [Authorize]
    public class AdminController(IAppointmentService appointmentService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appointments = await appointmentService.GetAllOrderedByDateAsync();
            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await appointmentService.GetByIdAsync(id);
            if (appointment is null)
                return NotFound();

            var model = new EditAppointmentViewModel
            {
                Id = appointment.Id,
                PatientName = appointment.PatientName,
                AppointmentDateTime = appointment.AppointmentDateTime,
                Description = appointment.Description,
                ContactNumber = appointment.ContactNumber,
                EmailAddress = appointment.EmailAddress
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var appointment = await appointmentService.GetByIdAsync(model.Id);
            if (appointment is null)
                return NotFound();

            appointment.PatientName = model.PatientName;
            appointment.AppointmentDateTime = model.AppointmentDateTime;
            appointment.Description = model.Description;
            appointment.ContactNumber = model.ContactNumber;
            appointment.EmailAddress = model.EmailAddress;

            await appointmentService.UpdateAsync(appointment);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            await appointmentService.ApproveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await appointmentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
