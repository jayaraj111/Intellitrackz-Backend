using AdminDashboard.DtoModels;

namespace AdminDashboard.Services.Interfaces;

public interface IDriverTripService
{
    Task<List<TodayTripResponse>> GetTodayTrips(int driverId, int companyId);
    Task<TripDetailsDto> StartTrip(int tripId, int driverId, int companyId);
    Task<TripDetailsDto> EndTrip(int tripId, int driverId, int companyId);
    Task<TripLocationLogResponse?> LogTripLocation(int tripId,int driverId,int companyId,TripLocationLogRequest request);
    Task<bool> MarkAttendanceAsync(int tripId, AttendanceRequestDto dto, int driverId, int companyId);
}
