using AdminDashboard.Models;

namespace AdminDashboard.Services.Interfaces;

public interface ITransportRouteService
{
    Task<IEnumerable<TransportRoute>> GetAllAsync(int companyId);
    Task<TransportRoute?> GetByIdAsync(int id, int companyId);
    Task<IEnumerable<TransportRoute>> SearchAsync(string? keyword, int companyId);
    Task<TransportRoute> CreateAsync(TransportRoute transportRoute);
    Task<TransportRoute?> UpdateAsync(int id, TransportRoute transportRoute, int companyId);
    Task<bool> DeleteAsync(int id, int companyId);
}
