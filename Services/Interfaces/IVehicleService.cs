using AdminDashboard.Models;

namespace AdminDashboard.Services.Interfaces;

public interface IVehicleService
{
    Task<IEnumerable<Vehicle>> GetAllAsync(int companyId);
    Task<Vehicle?> GetByIdAsync(int id, int companyId);
    Task<IEnumerable<Vehicle>> SearchAsync(string? keyword, int companyId); 
    Task<Vehicle> CreateAsync(Vehicle vehicle);
    Task<Vehicle?> UpdateAsync(int id, Vehicle vehicle, int companyId); 
    Task<bool> DeleteAsync(int id, int companyId); 
}
