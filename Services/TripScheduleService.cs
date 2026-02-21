using AdminDashboard.Data;
using AdminDashboard.DtoModels;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Services;

public class TripScheduleService : ITripScheduleService
{
    private readonly AppDbContext _context;
    public TripScheduleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TripSchedule>> GetAllAsync()
    {
        return await _context.TripSchedules
            .Include(x => x.Route)
            .Where(x => x.Status == 'Y') 
            .ToListAsync();
    }

    public async Task<TripSchedule?> GetByIdAsync(int id)
    {
        return await _context.TripSchedules
            .Include(x => x.Route)
            .FirstOrDefaultAsync(x => x.TripScheduleId == id);
    }

    //public async Task<TripSchedule> CreateAsync(TripScheduleDto dto)
    //{
    //    var schedule = new TripSchedule
    //    {
    //        TripName = dto.TripName,
    //        TripStartDate = dto.TripStartDate,
    //        TripEndDate = dto.TripEndDate,
    //        PlannedStartTime = TimeSpan.Parse(dto.PlannedStartTime),
    //        PlannedEndTime = string.IsNullOrWhiteSpace(dto.PlannedEndTime)
    //            ? null
    //            : TimeSpan.Parse(dto.PlannedEndTime),
    //        RouteId = dto.RouteId,
    //        Mon = dto.Mon,
    //        Tue = dto.Tue,
    //        Wed = dto.Wed,
    //        Thu = dto.Thu,
    //        Fri = dto.Fri,
    //        Sat = dto.Sat,
    //        Sun = dto.Sun,
    //        Status = 'Y'
    //    };

    //    _context.TripSchedules.Add(schedule);
    //    await _context.SaveChangesAsync();
    //    return schedule;
    //}

    //public async Task<TripSchedule> UpdateAsync(int id, TripScheduleDto dto)
    //{
    //    var existing = await _context.TripSchedules.FindAsync(id);
    //    if (existing == null) throw new Exception("Trip Schedule not found");

    //    existing.TripName = dto.TripName;
    //    existing.TripStartDate = dto.TripStartDate;
    //    existing.TripEndDate = dto.TripEndDate;
    //    existing.PlannedStartTime = TimeSpan.Parse(dto.PlannedStartTime);
    //    existing.PlannedEndTime = string.IsNullOrWhiteSpace(dto.PlannedEndTime)
    //        ? null
    //        : TimeSpan.Parse(dto.PlannedEndTime);
    //    existing.RouteId = dto.RouteId;
    //    existing.Mon = dto.Mon;
    //    existing.Tue = dto.Tue;
    //    existing.Wed = dto.Wed;
    //    existing.Thu = dto.Thu;
    //    existing.Fri = dto.Fri;
    //    existing.Sat = dto.Sat;
    //    existing.Sun = dto.Sun;

    //    await _context.SaveChangesAsync();
    //    return existing;
    //}

    public async Task<TripSchedule> CreateAsync(TripScheduleDto dto)
    {
        var routeStops = await _context.RouteStops
            .Where(x => x.RouteId == dto.RouteId)
            .OrderBy(x => x.StopOrder)
            .ToListAsync();

        TimeSpan? endTime = null;

        if (routeStops.Any())
        {
            endTime =
                TimeSpan.Parse(dto.PlannedStartTime)
                + routeStops.Last().PlannedArrivalTime;
        }

        var schedule = new TripSchedule
        {
            TripName = dto.TripName,
            TripStartDate = dto.TripStartDate,
            TripEndDate = dto.TripEndDate,
            PlannedStartTime = TimeSpan.Parse(dto.PlannedStartTime),
            PlannedEndTime = endTime,
            RouteId = dto.RouteId,
            Mon = dto.Mon,
            Tue = dto.Tue,
            Wed = dto.Wed,
            Thu = dto.Thu,
            Fri = dto.Fri,
            Sat = dto.Sat,
            Sun = dto.Sun,
            Status = 'Y'
        };

        _context.TripSchedules.Add(schedule);
        await _context.SaveChangesAsync();

        return schedule;
    }

    public async Task<TripSchedule> UpdateAsync(int id, TripScheduleDto dto)
    {
        var existing = await _context.TripSchedules.FindAsync(id)
            ?? throw new Exception("Trip not found");

        var routeStops = await _context.RouteStops
            .Where(x => x.RouteId == dto.RouteId)
            .OrderBy(x => x.StopOrder)
            .ToListAsync();

        existing.TripName = dto.TripName;
        existing.TripStartDate = dto.TripStartDate;
        existing.TripEndDate = dto.TripEndDate;
        existing.PlannedStartTime = TimeSpan.Parse(dto.PlannedStartTime);
        existing.RouteId = dto.RouteId;

        existing.PlannedEndTime =
            routeStops.Any()
            ? existing.PlannedStartTime +
              routeStops.Last().PlannedArrivalTime
            : null;

        existing.Mon = dto.Mon;
        existing.Tue = dto.Tue;
        existing.Wed = dto.Wed;
        existing.Thu = dto.Thu;
        existing.Fri = dto.Fri;
        existing.Sat = dto.Sat;
        existing.Sun = dto.Sun;

        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.TripSchedules.FindAsync(id);
        if (existing == null) return false;

        existing.Status = 'N'; // Soft delete
        _context.TripSchedules.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
