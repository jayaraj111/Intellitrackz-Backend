using AdminDashboard.DtoModels;
using AdminDashboard.Models;

namespace AdminDashboard.Services.Interfaces;

public interface ITripService
{
    Task<IEnumerable<object>> GetAllAsync(int companyId);
    Task<object?> GetByIdAsync(int id, int companyId);
    Task<IEnumerable<object>> SearchAsync(int companyId, string? q);

    Task<Trip> CreateAsync(TripDto dto, int companyId);
    Task<Trip?> UpdateAsync(int id, TripDto dto, int companyId);
    Task<bool> DeleteAsync(int id, int companyId);
    Task<int> GenerateFromScheduleAsync(int scheduleId,DateTime startDate,DateTime endDate,int companyId);
    Task<List<TripPathPointDto>> GetTripPathAsync(int tripId, int companyId);
}

