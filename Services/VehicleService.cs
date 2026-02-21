using AdminDashboard.Data;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace AdminDashboard.Services;

public class VehicleService : IVehicleService
{
    private readonly AppDbContext _context;

    public VehicleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync(int companyId)
    {
        return await _context.Vehicles.Where(rs => rs.CompanyId == companyId).ToListAsync();
    }

    public async Task<Vehicle?> GetByIdAsync(int id, int companyId)
    {
        return await _context.Vehicles
            .FirstOrDefaultAsync(rs => rs.VehicleId == id && rs.CompanyId == companyId);
    }
 
    public async Task<Vehicle> CreateAsync(Vehicle vehicle)
    {
        vehicle.CreatedAt = DateTime.UtcNow;
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task<IEnumerable<Vehicle>> SearchAsync(string? keyword, int companyId)
    {
        keyword = keyword?.Trim().ToLower();

        return await _context.Vehicles
            .Where(v => v.CompanyId == companyId) 
            .Where(v =>
                string.IsNullOrEmpty(keyword) ||
                v.RegistrationNumber.ToLower().Contains(keyword) ||
                v.Model.ToLower().Contains(keyword) ||
                v.ImeiId.ToLower().Contains(keyword)
            )
            .ToListAsync();
    }

    public async Task<Vehicle?> UpdateAsync(int id, Vehicle vehicle, int companyId)
    {
        var existing = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.VehicleId == id && v.CompanyId == companyId);

        if (existing == null) return null;
        existing.RegistrationNumber = vehicle.RegistrationNumber;
        existing.Status = vehicle.Status;
        existing.Model = vehicle.Model;
        existing.ImeiId = vehicle.ImeiId;
        existing.VehiclePhotoUrl = vehicle.VehiclePhotoUrl;
        existing.Remarks = vehicle.Remarks;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id, int companyId)
    {
        var existing = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.VehicleId == id && v.CompanyId == companyId);

        if (existing == null) return false;

        _context.Vehicles.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
