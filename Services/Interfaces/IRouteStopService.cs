using AdminDashboard.DtoModels;
using AdminDashboard.Models;

namespace AdminDashboard.Services.Interfaces;

public interface IRouteStopService
{
    Task<IEnumerable<Stop>> GetAllAsync(int companyId);
    Task<Stop?> GetByIdAsync(int id, int companyId);
    Task<IEnumerable<Stop>> SearchAsync(string? keyword, int companyId);
    Task<Stop> CreateAsync(Stop stop);
    Task<Stop?> UpdateAsync(int id, Stop stop, int companyId);
    Task<bool> DeleteAsync(int id, int companyId);
}
