using AdminDashboard.Data;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Services;

public class TransportRouteService : ITransportRouteService
{
    private readonly AppDbContext _context;

    public TransportRouteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransportRoute>> GetAllAsync(int companyId)
    {
        return await _context.Routes
            .Where(r => r.CompanyId == companyId)
            .ToListAsync();
    }

    public async Task<TransportRoute?> GetByIdAsync(int id, int companyId)
    {
        return await _context.Routes
            .FirstOrDefaultAsync(r => r.RouteId == id && r.CompanyId == companyId);
    }

    public async Task<IEnumerable<TransportRoute>> SearchAsync(string? keyword, int companyId)
    {
        keyword = keyword?.Trim().ToLower();

        return await _context.Routes
            .Where(r => r.CompanyId == companyId)
            .Where(r =>
                string.IsNullOrEmpty(keyword) ||
                r.RouteName.ToLower().Contains(keyword)
            )
            .ToListAsync();
    }

    public async Task<TransportRoute> CreateAsync(TransportRoute transportRoute)
    {
        transportRoute.CreatedAt = DateTime.UtcNow;
        _context.Routes.Add(transportRoute);
        await _context.SaveChangesAsync();
        return transportRoute;
    }

    public async Task<TransportRoute?> UpdateAsync(int id, TransportRoute transportRoute, int companyId)
    {
        var existing = await _context.Routes
            .FirstOrDefaultAsync(c => c.RouteId == id && c.CompanyId == companyId);

        if (existing == null)
            return null;

        existing.EndLocationLng = transportRoute.EndLocationLng; 
        existing.StartLocationLng = transportRoute.StartLocationLng;
        existing.StartLocationLat = transportRoute.StartLocationLat;
        existing.EndLocationLat = transportRoute.EndLocationLat;
        existing.RouteName = transportRoute.RouteName;
        existing.Status = transportRoute.Status;
       
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id, int companyId)
    {
        var existing = await _context.Routes
            .FirstOrDefaultAsync(r => r.RouteId == id && r.CompanyId == companyId);

        if (existing == null)
            return false;

        _context.Routes.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
