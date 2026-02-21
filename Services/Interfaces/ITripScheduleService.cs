using AdminDashboard.DtoModels;
using AdminDashboard.Models;

namespace AdminDashboard.Services.Interfaces;

public interface ITripScheduleService
{
    Task<List<TripSchedule>> GetAllAsync();
    Task<TripSchedule?> GetByIdAsync(int id);
    Task<TripSchedule> CreateAsync(TripScheduleDto dto);
    Task<TripSchedule> UpdateAsync(int id, TripScheduleDto dto);
    Task<bool> DeleteAsync(int id);
}

