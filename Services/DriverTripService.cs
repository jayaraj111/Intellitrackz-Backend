using AdminDashboard.Data;
using AdminDashboard.DtoModels;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace AdminDashboard.Services;


public class DriverTripService : IDriverTripService
{
    private readonly AppDbContext _context;

    public DriverTripService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TodayTripResponse>> GetTodayTrips(int driverId,int companyId)
    {
        var today = DateTime.Today;

        var trips = await _context.Trips
            .AsNoTracking()
            .Where(t =>
                t.DriverId == driverId &&
                t.CompanyId == companyId &&
                t.TripDate == today &&
                t.Status != 'X')
            .Select(t => new
            {
                t.TripId,
                t.TripDate,
                t.SessionName,
                t.Status,
                Route = new
                {
                    t.Route.RouteId,
                    t.Route.RouteName,
                    t.Route.StartLocationLat,
                    t.Route.StartLocationLng,
                    t.Route.EndLocationLat,
                    t.Route.EndLocationLng
                },
                Vehicle = new
                {
                    t.Vehicle.VehicleId,
                    t.Vehicle.RegistrationNumber,
                    t.Vehicle.ImeiId,
                    t.Vehicle.VehiclePhotoUrl
                }
            })
            .ToListAsync();

        var response = new List<TodayTripResponse>();

        foreach (var trip in trips)
        {
            var stops = await _context.RouteStops
                .AsNoTracking()
                .Where(rs => rs.RouteId == trip.Route.RouteId)
                .OrderBy(rs => rs.StopOrder)
                .Select(rs => new StopWithPassengersDto
                {
                    StopId = rs.StopId,
                    StopName = _context.Stops
                        .Where(s => s.StopId == rs.StopId)
                        .Select(s => s.StopName)
                        .FirstOrDefault() ?? "",
                    Latitude = _context.Stops
                        .Where(s => s.StopId == rs.StopId)
                        .Select(s => s.Latitude)
                        .FirstOrDefault().ToString() ?? "",
                    Longitude = _context.Stops
                        .Where(s => s.StopId == rs.StopId)
                        .Select(s => s.Longitude)
                        .FirstOrDefault().ToString() ?? "",
                    PlannedArrivalTime = rs.PlannedArrivalTime,

                    //Passengers = _context.RoutePassengers
                    //    .Where(rp =>
                    //        rp.RouteId == rs.RouteId &&
                    //        rp.StopId == rs.StopId &&
                    //        rp.Status == 'Y')
                    //    .Select(rp => new PassengerDto
                    //    {
                    //        PassengerId = rp.UserId,
                    //        PassengerName = rp.User!.FullName
                    //    })
                    //    .ToList()
                    Passengers = _context.RoutePassengers
    .Where(rp =>
        rp.RouteId == rs.RouteId &&
        rp.StopId == rs.StopId &&
        rp.Status == 'Y')
    .Select(rp => new PassengerDto
    {
        PassengerId = rp.UserId,
        PassengerName = rp.User!.FullName,

        IsBoarded = _context.TripAttendances
            .Where(a =>
                a.TripId == trip.TripId &&
                a.PassengerId == rp.UserId &&
                a.StopId == rs.StopId)
            .Select(a => (bool?)a.IsBoarded)
            .FirstOrDefault()
    })
    .ToList()
                })
                .ToListAsync();

            response.Add(new TodayTripResponse
            {
                TripId = trip.TripId,
                TripDate = trip.TripDate,
                Session = trip.SessionName,
                Status = trip.Status.ToString(),

                Route = new RouteDto
                {
                    RouteId = trip.Route.RouteId,
                    RouteName = trip.Route.RouteName,
                    StartLocationLat = trip.Route.StartLocationLat.ToString(),
                    StartLocationLng = trip.Route.StartLocationLng.ToString(),
                    EndLocationLat = trip.Route.EndLocationLat.ToString(),
                    EndLocationLng = trip.Route.EndLocationLng.ToString()
                },

                Vehicle = new VehicleDto
                {
                    VehicleId = trip.Vehicle.VehicleId,
                    RegistrationNumber = trip.Vehicle.RegistrationNumber,
                    ImeiId = trip.Vehicle.ImeiId,
                    PhotoUrl = trip.Vehicle.VehiclePhotoUrl,
                },

                Stops = stops
            });
        }

        return response;
    }

    public async Task<TripDetailsDto?> StartTrip(int tripId,int driverId,int companyId)
    {
        var trip = await _context.Trips.FirstOrDefaultAsync(t =>
            t.TripId == tripId &&
            t.DriverId == driverId &&
            t.CompanyId == companyId &&
            t.Status == 'P');

        if (trip == null)
            return null;

        trip.Status = 'S';
        trip.ActualStartTime = DateTime.Now;

        await _context.SaveChangesAsync();

        return new TripDetailsDto
        {
            TripId = trip.TripId,
            TripDate = trip.TripDate,
            Session = trip.SessionName,
            Status = trip.Status.ToString(),
            ActualStartTime = trip.ActualStartTime
        };
    }

    public async Task<TripDetailsDto?> EndTrip(int tripId,int driverId,int companyId)
    {
        var trip = await _context.Trips.FirstOrDefaultAsync(t =>
            t.TripId == tripId &&
            t.DriverId == driverId &&
            t.CompanyId == companyId &&
            t.Status == 'S');

        if (trip == null)
            return null;

        trip.Status = 'C';
        trip.ActualEndTime = DateTime.Now;

        await _context.SaveChangesAsync();

        return new TripDetailsDto
        {
            TripId = trip.TripId,
            TripDate = trip.TripDate,
            Session = trip.SessionName,
            Status = trip.Status.ToString(),
            ActualEndTime = trip.ActualEndTime
        };
    }

    public async Task<TripLocationLogResponse?> LogTripLocation(int tripId, int driverId, int companyId, TripLocationLogRequest request)
    {
        bool validTrip = await _context.Trips.AnyAsync(t =>
    t.TripId == tripId &&
    t.DriverId == driverId &&
    t.CompanyId == companyId &&
    (t.Status == 'S' || t.Status == 'P'));

        if (!validTrip)
            return null;

        var log = new TripLocationLog
        {
            TripId = tripId,
            CompanyId = companyId,
            DriverId = driverId,
            Latitude = Convert.ToDouble(request.Latitude),
            Longitude = Convert.ToDouble(request.Longitude)
        };

        _context.TripLocationLogs.Add(log);
        await _context.SaveChangesAsync();

        return new TripLocationLogResponse
        {
            Id = log.TripLocationLogId,
            TripId = log.TripId,
            Latitude = Convert.ToDouble(log.Latitude),
            Longitude = Convert.ToDouble(log.Longitude),
            LoggedAt = log.LoggedAt
        };
    }

    public async Task<bool> MarkAttendanceAsync(int tripId,AttendanceRequestDto dto,int driverId,int companyId)
    {
        var trip = await _context.Trips
            .FirstOrDefaultAsync(t =>
                t.TripId == tripId &&
                t.DriverId == driverId &&
                t.CompanyId == companyId &&
                t.Status == 'S');

        if (trip == null)
            throw new Exception("Invalid Trip");

        foreach (var stop in dto.Stops)
        {
            foreach (var p in stop.Passengers)
            {
                var existing =
                    await _context.TripAttendances
                    .FirstOrDefaultAsync(x =>
                        x.TripId == tripId &&
                        x.PassengerId == p.PassengerId);

                if (existing != null)
                {
                    existing.IsBoarded = p.IsBoarded;
                    existing.MarkedAt = DateTime.UtcNow;
                    existing.MarkedBy = driverId;
                }
                else
                {
                    _context.TripAttendances.Add(
                        new TripAttendance
                        {
                            TripId = tripId,
                            StopId = stop.StopId,
                            PassengerId = p.PassengerId,
                            IsBoarded = p.IsBoarded,
                            MarkedAt = DateTime.UtcNow,
                            MarkedBy = driverId,
                            CompanyId = companyId
                        });
                }
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

   

}

