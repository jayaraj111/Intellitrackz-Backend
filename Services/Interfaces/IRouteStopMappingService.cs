using AdminDashboard.DtoModels;

namespace AdminDashboard.Services.Interfaces;

public interface IRouteStopMappingService
{
    Task<List<RouteStopMappingListDto>> GetAllAsync();
    Task<RouteStopMappingDetailDto?> GetByRouteIdAsync(int routeId);
    Task<List<RouteStopMappingListDto>> SearchAsync(string? q);
    Task<bool> SaveRouteStopsMappingAsync(RouteStopMappingDto mappingDto);
    Task<bool> DeleteByRouteIdAsync(int routeId);
}