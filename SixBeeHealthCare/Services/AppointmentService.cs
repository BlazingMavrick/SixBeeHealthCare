using NPoco;
using SixBeeHealthCare.Web.Models;
using SixBeeHealthCare.Web.Services.Interfaces;

namespace SixBeeHealthCare.Web.Services
{
    public class AppointmentService(IDatabase db) : IAppointmentService
    {
        public async Task<IEnumerable<Appointment>> GetAllOrderedByDateAsync() =>
            await db.FetchAsync<Appointment>(
                "SELECT * FROM Appointments ORDER BY AppointmentDateTime");

        public async Task<Appointment?> GetByIdAsync(int id) =>
            await db.SingleOrDefaultAsync<Appointment>(
                "SELECT * FROM Appointments WHERE Id = @0", id);

        public async Task CreateAsync(Appointment appointment)
        {
            appointment.CreatedAt = DateTime.UtcNow;
            await db.InsertAsync(appointment);
        }

        public async Task UpdateAsync(Appointment appointment) =>
            await db.UpdateAsync(appointment);

        public async Task ApproveAsync(int id)
        {
            var appointment = await db.SingleOrDefaultAsync<Appointment>(
                "SELECT * FROM Appointments WHERE Id = @0", id);

            if (appointment is null) return;

            appointment.IsApproved = !appointment.IsApproved;
            await db.UpdateAsync(appointment);
        }

        public async Task DeleteAsync(int id) =>
            await db.ExecuteAsync("DELETE FROM Appointments WHERE Id = @0", id);
    }
}
