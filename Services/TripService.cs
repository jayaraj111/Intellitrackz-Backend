using AdminDashboard.Data;
using AdminDashboard.DtoModels;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TripService : ITripService
{
    private readonly AppDbContext _context;

    public TripService(AppDbContext context)
    {
        _context = context;
    }

    //public async Task<IEnumerable<object>> GetAllAsync(int companyId)
    //{
    //    return await _context.Trips
    //        .Where(t => t.CompanyId == companyId)
    //        .Include(t => t.Route)
    //        .Include(t => t.Vehicle)
    //        .Include(t => t.Driver)
    //        .Select(t => new
    //        {
    //            t.TripId,
    //            t.TripDate,
    //            t.SessionName,
    //            t.Status,
    //            Route = t.Route!.RouteName,
    //            Vehicle = t.Vehicle!.RegistrationNumber,
    //            Driver = t.Driver!.FullName
    //        })
    //        .ToListAsync();
    //}
    public async Task<IEnumerable<object>> GetAllAsync(int companyId)
    {
        return await _context.Trips
            .Where(t => t.CompanyId == companyId)
            // Sort by date descending before projection
            .OrderByDescending(t => t.TripDate)
            // Optional: Secondary sort by ID to keep latest entries first if dates match
            .ThenByDescending(t => t.TripId)
            .Select(t => new
            {
                t.TripId,
                t.TripDate,
                t.SessionName,
                t.Status,
                // Using null-conditional access/coalescing for safety
                Route = t.Route != null ? t.Route.RouteName : "N/A",
                Vehicle = t.Vehicle != null ? t.Vehicle.RegistrationNumber : "Not Assigned",
                Driver = t.Driver != null ? t.Driver.FullName : "Not Assigned"
            })
            .ToListAsync();
    }

    public async Task<object?> GetByIdAsync(int id, int companyId)
    {
        return await _context.Trips
            .Where(t => t.TripId == id && t.CompanyId == companyId)
            .Select(t => new
            {
                t.TripId,
                t.TripDate,
                t.SessionName,
                t.PlannedStartTime,
                t.PlannedEndTime,
                t.RouteId,
                t.VehicleId,
                t.DriverId,
                t.Status
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<object>> SearchAsync(int companyId, string? q)
    {
        var query = _context.Trips.Where(t => t.CompanyId == companyId);

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.ToLower();
            query = query.Where(t =>
                t.SessionName.ToLower().Contains(q) ||
                (t.Route != null && t.Route.RouteName.ToLower().Contains(q)) ||
                (t.Driver != null && t.Driver.FullName.ToLower().Contains(q))
            );
        }

        // IMPORTANT: Project to the same shape as GetAllAsync
        return await query
            .OrderByDescending(t => t.TripDate)
            .Select(t => new
            {
                t.TripId,
                t.TripDate,
                t.SessionName,
                t.Status,
                Route = t.Route != null ? t.Route.RouteName : "N/A",
                Vehicle = t.Vehicle != null ? t.Vehicle.RegistrationNumber : "Not Assigned",
                Driver = t.Driver != null ? t.Driver.FullName : "Not Assigned"
            })
            .ToListAsync();
    }

    public async Task<Trip> CreateAsync(TripDto dto, int companyId)
    {
        var trip = new Trip
        {
            CompanyId = companyId,
            RouteId = dto.RouteId,
            VehicleId = dto.VehicleId,
            DriverId = dto.DriverId,
            TripDate = dto.TripDate,
            SessionName = dto.SessionName,
            PlannedStartTime = dto.PlannedStartTime,
            PlannedEndTime = dto.PlannedEndTime,
            Status = 'P'
        };

        _context.Trips.Add(trip);
        await _context.SaveChangesAsync();
        return trip;
    }

    public async Task<Trip?> UpdateAsync(int id, TripDto dto, int companyId)
    {
        var trip = await _context.Trips
            .FirstOrDefaultAsync(t => t.TripId == id && t.CompanyId == companyId);

        if (trip == null) return null;

        if (trip.Status != 'P')
            throw new Exception("Cannot edit started or completed trip");

        trip.RouteId = dto.RouteId;
        trip.VehicleId = dto.VehicleId;
        trip.DriverId = dto.DriverId;
        trip.TripDate = dto.TripDate;
        trip.SessionName = dto.SessionName;
        trip.PlannedStartTime = dto.PlannedStartTime;
        trip.PlannedEndTime = dto.PlannedEndTime;

        await _context.SaveChangesAsync();
        return trip;
    }

    public async Task<bool> DeleteAsync(int id, int companyId)
    {
        var trip = await _context.Trips
            .FirstOrDefaultAsync(t => t.TripId == id && t.CompanyId == companyId);

        if (trip == null) return false;

        if (trip.Status != 'P')
            throw new Exception("Cannot delete started/completed trip");

        _context.Trips.Remove(trip);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GenerateFromScheduleAsync(
     int scheduleId,
     DateTime startDate,
     DateTime endDate,
     int companyId)
    {
        var schedule = await _context.TripSchedules
            .FirstOrDefaultAsync(x => x.TripScheduleId == scheduleId);

        if (schedule == null)
            return 0;

        int createdCount = 0;

        for (var date = startDate.Date;
             date <= endDate.Date;
             date = date.AddDays(1))
        {
            bool shouldRun =
                (date.DayOfWeek == DayOfWeek.Monday && schedule.Mon) ||
                (date.DayOfWeek == DayOfWeek.Tuesday && schedule.Tue) ||
                (date.DayOfWeek == DayOfWeek.Wednesday && schedule.Wed) ||
                (date.DayOfWeek == DayOfWeek.Thursday && schedule.Thu) ||
                (date.DayOfWeek == DayOfWeek.Friday && schedule.Fri) ||
                (date.DayOfWeek == DayOfWeek.Saturday && schedule.Sat) ||
                (date.DayOfWeek == DayOfWeek.Sunday && schedule.Sun);

            if (!shouldRun)
                continue;

            bool exists = await _context.Trips.AnyAsync(t =>
                t.CompanyId == companyId &&
                t.RouteId == schedule.RouteId &&
                t.TripDate.Date == date.Date &&
                t.SessionName.StartsWith(schedule.TripName));

            if (exists)
                continue;

            _context.Trips.Add(new Trip
            {
                CompanyId = companyId,
                RouteId = schedule.RouteId,
                TripDate = date,
                SessionName =
                    $"{schedule.TripName}_{date:ddMMMyy}",

                PlannedStartTime = schedule.PlannedStartTime,
                PlannedEndTime = schedule.PlannedEndTime,

                VehicleId = null,
                DriverId = null,

                Status = 'P'
            });

            createdCount++;
        }

        await _context.SaveChangesAsync();

        return createdCount;
    }
}
