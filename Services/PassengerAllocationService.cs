//using AdminDashboard.Data;
//using AdminDashboard.DtoModels;
//using AdminDashboard.Models;
//using AdminDashboard.Services.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace AdminDashboard.Services;

//public class PassengerAllocationService : IPassengerAllocationService
//{
//    private readonly AppDbContext _context;

//    public PassengerAllocationService(AppDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<IEnumerable<object>> GetAllAsync(int companyId)
//    {
//        return await _context.RoutePassengers
//            .Where(x => x.CompanyId == companyId)
//            .Include(x => x.User)
//            .Include(x => x.Route)
//            .Include(x => x.Stop)
//            .Select(x => new
//            {
//                x.RoutePassengerId,
//                PassengerName = x.User!.FullName,
//                RouteName = x.Route!.RouteName,
//              //  StopName = x.Stop!.StopName,
//                x.Status
//            })
//            .ToListAsync();
//    }

//    //public async Task<IEnumerable<object>> GetAllAsync(int companyId)
//    //{
//    //    return await _context.RoutePassengers
//    //        .Where(x => x.CompanyId == companyId)
//    //        .Include(x => x.User)
//    //        .Include(x => x.Route)
//    //        .Include(x => x.RouteStop) // The mapping table
//    //            .ThenInclude(rs => rs.Stop) // The actual Stop entity
//    //        .Select(x => new
//    //        {
//    //            x.RoutePassengerId,
//    //            PassengerName = x.User != null ? x.User.FullName : "Unknown",
//    //            RouteName = x.Route != null ? x.Route.RouteName : "N/A",

//    //            // INCORRECT: x.RouteStop.StopName
//    //            // CORRECT: Reach through the Stop navigation property
//    //            StopName = x.RouteStop != null && x.RouteStop.Stop != null
//    //                       ? x.RouteStop.Stop.StopName
//    //                       : "N/A",

//    //            x.Status
//    //        })
//    //        .ToListAsync();
//    //}

//    public async Task<IEnumerable<object>> SearchAsync(int companyId, string? q)
//    {
//        var query = _context.RoutePassengers
//            .Include(x => x.User)
//            .Include(x => x.Route)
//            .Include(x => x.Stop)
//            .Where(x => x.CompanyId == companyId);

//        if (!string.IsNullOrWhiteSpace(q))
//        {
//            q = q.ToLower();

//            //query = query.Where(x =>
//            //    x.User!.FullName.ToLower().Contains(q) ||
//            //  //  x.Route!.RouteName.ToLower().Contains(q) ||
//            //  //  x.Stop!.StopName.ToLower().Contains(q)
//            //);
//        }

//        return await query.Select(x => new
//        {
//            x.RoutePassengerId,
//            PassengerName = x.User!.FullName,
//           // RouteName = x.Route!.RouteName,
//           // StopName = x.Stop!.StopName,
//            x.Status
//        }).ToListAsync();
//    }

//    public async Task<bool> SyncRouteAllocationsAsync(BulkPassengerAllocationDto dto, int companyId)
//    {
//        // 1. Validate the Route exists
//        var routeExists = await _context.Routes.AnyAsync(r => r.RouteId == dto.RouteId && r.CompanyId == companyId);
//        if (!routeExists) throw new Exception("Invalid Route ID");

//        // 2. Remove old allocations for this specific route
//        var existingAllocations = await _context.RoutePassengers
//            .Where(x => x.RouteId == dto.RouteId && x.CompanyId == companyId)
//            .ToListAsync();
//        _context.RoutePassengers.RemoveRange(existingAllocations);

//        // 3. Map and Validate new entities
//        var newEntities = new List<RoutePassenger>();
//        foreach (var a in dto.Allocations)
//        {
//            // Ensure IDs are greater than 0 to avoid FK conflicts with default values
//            if (a.UserId > 0 && a.StopId > 0)
//            {
//                newEntities.Add(new RoutePassenger
//                {
//                    CompanyId = companyId,
//                    RouteId = dto.RouteId,
//                    UserId = a.UserId,
//                    StopId = a.StopId,
//                    Status = 'Y'
//                });
//            }
//        }

//        if (newEntities.Any())
//        {
//            await _context.RoutePassengers.AddRangeAsync(newEntities);
//        }

//        try
//        {
//            return await _context.SaveChangesAsync() > 0;
//        }
//        catch (DbUpdateException ex)
//        {
//            // This will help you see exactly which ID is causing the crash in the debugger
//            Console.WriteLine(ex.InnerException?.Message);
//            throw;
//        }
//    }

//    public async Task<object?> GetByIdAsync(int id, int companyId)
//    {
//        return await _context.RoutePassengers
//            .Where(x => x.RoutePassengerId == id && x.CompanyId == companyId)
//            .Select(x => new PassengerAllocationDto
//            {
//                UserId = x.UserId,
//                RouteId = x.RouteId,
//                StopId = x.StopId
//            })
//            .FirstOrDefaultAsync();
//    }

//    public async Task<IEnumerable<PassengerAllocationDto>> GetByRouteIdAllocationsAsync(int routeId, int companyId)
//    {
//        return await _context.RoutePassengers
//            .Where(x => x.RouteId == routeId && x.CompanyId == companyId)
//            .Select(x => new PassengerAllocationDto
//            {
//                UserId = x.UserId,
//                RouteId = x.RouteId,
//                StopId = x.StopId // This should be the RouteStopId as discussed previously
//            })
//            .ToListAsync(); // Return a list, not FirstOrDefault
//    }


//    public async Task<RoutePassenger?> UpdateAsync(int id, PassengerAllocationDto dto, int companyId)
//    {
//        var entity = await _context.RoutePassengers
//            .FirstOrDefaultAsync(x => x.RoutePassengerId == id && x.CompanyId == companyId);

//        if (entity == null)
//            return null;

//        entity.UserId = dto.UserId;
//        entity.RouteId = dto.RouteId;
//        entity.StopId = dto.StopId;

//        await _context.SaveChangesAsync();
//        return entity;
//    }


//    public async Task<bool> DeleteAsync(int id, int companyId)
//    {
//        var entity = await _context.RoutePassengers
//            .FirstOrDefaultAsync(x => x.RoutePassengerId == id && x.CompanyId == companyId);

//        if (entity == null)
//            return false;

//        _context.RoutePassengers.Remove(entity);
//        await _context.SaveChangesAsync();
//        return true;
//    }
//}

using AdminDashboard.Data;
using AdminDashboard.DtoModels;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Services;

public class PassengerAllocationService : IPassengerAllocationService
{
    private readonly AppDbContext _context;

    public PassengerAllocationService(AppDbContext context)
    {
        _context = context;
    }

    // ============================
    // GET ALL (Listing Screen)
    // ============================
    public async Task<IEnumerable<object>> GetAllAsync(int companyId)
    {
        return await _context.RoutePassengers
            .AsNoTracking()
            .Where(x => x.CompanyId == companyId)
            .Select(x => new
            {
                x.RoutePassengerId,
                PassengerName = x.User!.FullName,
                RouteName = x.Route!.RouteName,
                StopName = x.Stop!.StopName,
                x.Status
            })
            .ToListAsync();
    }

    // ============================
    // GET BY ROUTE (Edit Screen)
    // ============================
    public async Task<IEnumerable<PassengerAllocationDto>> GetByRouteIdAllocationsAsync(
        int routeId,
        int companyId)
    {
        return await _context.RoutePassengers
            .AsNoTracking()
            .Where(x => x.RouteId == routeId && x.CompanyId == companyId)
            .Select(x => new PassengerAllocationDto
            {
                UserId = x.UserId,
                RouteId = x.RouteId,
                StopId = x.StopId // ✅ MASTER StopId ONLY
            })
            .ToListAsync();
    }

    // ============================
    // BULK SYNC (SAVE)
    // ============================
    public async Task<bool> SyncRouteAllocationsAsync(
        BulkPassengerAllocationDto dto,
        int companyId)
    {
        // 1️⃣ Validate Route
        bool routeExists = await _context.Routes
            .AnyAsync(r => r.RouteId == dto.RouteId && r.CompanyId == companyId);

        if (!routeExists)
            throw new Exception("Invalid Route");

        // 2️⃣ Remove old allocations for this route
        var oldAllocations = await _context.RoutePassengers
            .Where(x => x.RouteId == dto.RouteId && x.CompanyId == companyId)
            .ToListAsync();

        _context.RoutePassengers.RemoveRange(oldAllocations);

        // 3️⃣ Insert new allocations (WITH VALIDATION)
        foreach (var a in dto.Allocations)
        {
            if (a.UserId <= 0 || a.StopId <= 0)
                continue;

            // 🔒 CRITICAL VALIDATION
            bool stopIsValid = await _context.RouteStops.AnyAsync(rs =>
                rs.RouteId == dto.RouteId &&
                rs.StopId == a.StopId);

            if (!stopIsValid)
                throw new Exception($"Invalid StopId {a.StopId} for Route {dto.RouteId}");

            _context.RoutePassengers.Add(new RoutePassenger
            {
                CompanyId = companyId,
                RouteId = dto.RouteId,
                UserId = a.UserId,
                StopId = a.StopId, 
                Status = 'Y'
            });
        }

        return await _context.SaveChangesAsync() > 0;
    }

    // ============================
    // GET BY ID
    // ============================
    public async Task<object?> GetByIdAsync(int id, int companyId)
    {
        return await _context.RoutePassengers
            .AsNoTracking()
            .Where(x => x.RoutePassengerId == id && x.CompanyId == companyId)
            .Select(x => new PassengerAllocationDto
            {
                UserId = x.UserId,
                RouteId = x.RouteId,
                StopId = x.StopId
            })
            .FirstOrDefaultAsync();
    }

    // ============================
    // UPDATE
    // ============================
    public async Task<RoutePassenger?> UpdateAsync(
        int id,
        PassengerAllocationDto dto,
        int companyId)
    {
        var entity = await _context.RoutePassengers
            .FirstOrDefaultAsync(x => x.RoutePassengerId == id && x.CompanyId == companyId);

        if (entity == null)
            return null;

        // Validate stop belongs to route
        bool stopIsValid = await _context.RouteStops.AnyAsync(rs =>
            rs.RouteId == dto.RouteId &&
            rs.StopId == dto.StopId);

        if (!stopIsValid)
            throw new Exception("Invalid stop for route");

        entity.UserId = dto.UserId;
        entity.RouteId = dto.RouteId;
        entity.StopId = dto.StopId;

        await _context.SaveChangesAsync();
        return entity;
    }

    // ============================
    // DELETE
    // ============================
    public async Task<bool> DeleteAsync(int id, int companyId)
    {
        var entity = await _context.RoutePassengers
            .FirstOrDefaultAsync(x => x.RoutePassengerId == id && x.CompanyId == companyId);

        if (entity == null)
            return false;

        _context.RoutePassengers.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<object>> SearchAsync(int companyId, string? q)
    {
        var query = _context.RoutePassengers
            .AsNoTracking()
            .Where(x => x.CompanyId == companyId);

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.ToLower();

            query = query.Where(x =>
                x.User!.FullName.ToLower().Contains(q) ||
                x.Route!.RouteName.ToLower().Contains(q) ||
                x.Stop!.StopName.ToLower().Contains(q)
            );
        }

        return await query
            .Select(x => new
            {
                x.RoutePassengerId,
                PassengerName = x.User!.FullName,
                RouteName = x.Route!.RouteName,
                StopName = x.Stop!.StopName,
                x.Status
            })
            .ToListAsync();
    }

}
