using AdminDashboard.Models;

namespace AdminDashboard.Services.Interfaces;

public interface IVehicleDocumentService
{
    Task<IEnumerable<VehicleDocument>> GetByVehicleAsync(int vehicleId, int companyId);
    Task<VehicleDocument> CreateAsync(VehicleDocument doc, int companyId);
    Task<bool> DeleteAsync(int id, int companyId);
}
