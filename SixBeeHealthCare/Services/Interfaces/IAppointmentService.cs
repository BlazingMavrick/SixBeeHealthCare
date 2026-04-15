using SixBeeHealthCare.Web.Models;

namespace SixBeeHealthCare.Web.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllOrderedByDateAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task CreateAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task ApproveAsync(int id);
        Task DeleteAsync(int id);
    }
}
