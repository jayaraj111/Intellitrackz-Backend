using AdminDashboard.Data;
using AdminDashboard.DtoModels;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Services;

public class RouteStopMappingService : IRouteStopMappingService
{
    private readonly AppDbContext _context;

    public RouteStopMappingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<RouteStopMappingListDto>> GetAllAsync()
    {
        var routes = await _context.Routes
            .Include(r => r.RouteStops)
                .ThenInclude(rs => rs.Stop)
            .ToListAsync();

        return routes
            .Where(r => r.RouteStops.Any())
            .Select(r => new RouteStopMappingListDto
            {
                RouteId = r.RouteId,
                RouteName = r.RouteName,
                TotalStops = r.RouteStops.Count,
                FirstStop = r.RouteStops.OrderBy(x => x.StopOrder).First().Stop!.StopName,
                LastStop = r.RouteStops.OrderByDescending(x => x.StopOrder).First().Stop!.StopName
            }).ToList();
    }

    public async Task<RouteStopMappingDetailDto?> GetByRouteIdAsync(int routeId)
    {
        var route = await _context.Routes
            .Where(r => r.RouteId == routeId)
            .Include(r => r.RouteStops)
            .ThenInclude(rs => rs.Stop)
            .FirstOrDefaultAsync();

        if (route == null) return null;

        return new RouteStopMappingDetailDto
        {
            RouteId = route.RouteId,
            Stops = route.RouteStops
    .OrderBy(x => x.StopOrder)
    .Select(x => new RouteStopItemDto
    {
        RouteStopId = x.RouteStopId, 
        StopId = x.StopId,
        StopName = x.Stop!.StopName,
        StopOrder = x.StopOrder,
        PlannedArrivalTime = x.PlannedArrivalTime.ToString(@"hh\:mm")
    }).ToList()
        };
    }

    public async Task<List<RouteStopMappingListDto>> SearchAsync(string? q)
    {
        var query = _context.Routes
            .Include(r => r.RouteStops)
                .ThenInclude(rs => rs.Stop)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.ToLower();
            query = query.Where(r =>
                r.RouteName.ToLower().Contains(q) ||
                r.RouteStops.Any(rs => rs.Stop!.StopName.ToLower().Contains(q))
            );
        }

        var routes = await query.ToListAsync();

        return routes
            .Where(r => r.RouteStops.Any())
            .Select(r => new RouteStopMappingListDto
            {
                RouteId = r.RouteId,
                RouteName = r.RouteName,
                TotalStops = r.RouteStops.Count,
                FirstStop = r.RouteStops
                    .OrderBy(x => x.StopOrder)
                    .First().Stop!.StopName,
                LastStop = r.RouteStops
                    .OrderByDescending(x => x.StopOrder)
                    .First().Stop!.StopName
            }).ToList();
    }

    public async Task<bool> SaveRouteStopsMappingAsync(RouteStopMappingDto mappingDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var existingStops = _context.RouteStops
                .Where(rs => rs.RouteId == mappingDto.RouteId);

            _context.RouteStops.RemoveRange(existingStops);

            var newMappings = mappingDto.Stops.Select(s => new RouteStop
            {
                RouteId = mappingDto.RouteId,
                StopId = s.StopId,
                StopOrder = s.StopOrder,

                PlannedArrivalTime =
                    TimeSpan.TryParse(s.PlannedArrivalTime, out var t)
                        ? t
                        : TimeSpan.Zero

            }).ToList();

            await _context.RouteStops.AddRangeAsync(newMappings);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            Console.WriteLine("Route stop save error:");
            Console.WriteLine(ex.ToString());

            return false;
        }
    }


    public async Task<bool> DeleteByRouteIdAsync(int routeId)
    {
        var mappings = await _context.RouteStops
            .Where(x => x.RouteId == routeId)
            .ToListAsync();

        if (!mappings.Any()) return false;

        _context.RouteStops.RemoveRange(mappings);
        await _context.SaveChangesAsync();
        return true;
    }

}
