using AdminDashboard.DtoModels;
using AdminDashboard.Models;

namespace AdminDashboard.Services.Interfaces;

public interface IPassengerAllocationService
{
    Task<IEnumerable<object>> GetAllAsync(int companyId);
    Task<object?> GetByIdAsync(int id, int companyId);
    Task<IEnumerable<PassengerAllocationDto>> GetByRouteIdAllocationsAsync(int routeId, int companyId);

    Task<IEnumerable<object>> SearchAsync(int companyId, string? q);
    Task<bool> SyncRouteAllocationsAsync(BulkPassengerAllocationDto dto, int companyId);
    Task<RoutePassenger?> UpdateAsync(int id, PassengerAllocationDto dto, int companyId);
    Task<bool> DeleteAsync(int id, int companyId);
}

