using AdminDashboard.Data;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Services;

public class VehicleDocumentService : IVehicleDocumentService
{
    private readonly AppDbContext _context;

    public VehicleDocumentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VehicleDocument>> GetByVehicleAsync(int vehicleId, int companyId)
    {
        return await _context.VehicleDocuments
            .Include(d => d.Vehicle)
            .Where(d => d.VehicleId == vehicleId &&
                        d.Vehicle.CompanyId == companyId)
            .ToListAsync();
    }

    public async Task<VehicleDocument> CreateAsync(VehicleDocument doc, int companyId)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v =>
                v.VehicleId == doc.VehicleId &&
                v.CompanyId == companyId);

        if (vehicle == null)
            throw new Exception("Vehicle not found");

        _context.VehicleDocuments.Add(doc);
        await _context.SaveChangesAsync();

        return doc;
    }

    public async Task<bool> DeleteAsync(int id, int companyId)
    {
        var doc = await _context.VehicleDocuments
            .Include(d => d.Vehicle)
            .FirstOrDefaultAsync(d =>
                d.VehicleDocumentId == id &&
                d.Vehicle.CompanyId == companyId);

        if (doc == null) return false;

        _context.VehicleDocuments.Remove(doc);
        await _context.SaveChangesAsync();

        return true;
    }
}
