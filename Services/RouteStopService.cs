using AdminDashboard.Data;
using AdminDashboard.DtoModels;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace AdminDashboard.Services;

public class RouteStopService : IRouteStopService
{
    private readonly AppDbContext _context;

    public RouteStopService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Stop>> GetAllAsync(int companyId)
    {
        return await _context.Stops
            .Where(s => s.CompanyId == companyId)
            .ToListAsync();
    }

    public async Task<Stop?> GetByIdAsync(int id, int companyId)
    {
        return await _context.Stops
            .FirstOrDefaultAsync(s => s.StopId == id && s.CompanyId == companyId);
    }

    public async Task<IEnumerable<Stop>> SearchAsync(string? keyword, int companyId)
    {
        keyword = keyword?.Trim().ToLower();

        return await _context.Stops
            .Where(s => s.CompanyId == companyId)
            .Where(s => string.IsNullOrEmpty(keyword) || s.StopName.ToLower().Contains(keyword))
            .ToListAsync();
    }

    public async Task<Stop> CreateAsync(Stop stop)
    {
        stop.CreatedAt = DateTime.UtcNow;
        _context.Stops.Add(stop);
        await _context.SaveChangesAsync();
        return stop;
    }

    public async Task<Stop?> UpdateAsync(int id, Stop stop, int companyId)
    {
        var existing = await _context.Stops
            .FirstOrDefaultAsync(s => s.StopId == id && s.CompanyId == companyId);

        if (existing == null) return null;

        existing.StopName = stop.StopName;
        existing.Latitude = stop.Latitude;
        existing.Longitude = stop.Longitude;
        existing.Status = stop.Status;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id, int companyId)
    {
        var existing = await _context.Stops
            .FirstOrDefaultAsync(s => s.StopId == id && s.CompanyId == companyId);

        if (existing == null) return false;

        _context.Stops.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> StopExistsAsync(string stopName,int companyId)
    {
        var normalized = stopName.Trim().ToLower();

        return await _context.Stops
            .AnyAsync(s =>
                s.CompanyId == companyId &&
                s.StopName.ToLower().Trim() == normalized);
    }
   
}
