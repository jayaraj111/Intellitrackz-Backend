using AdminDashboard.DtoModels;
using AdminDashboard.Services;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AdminDashboard.Controllers;

[ApiController]
[Route("api/driver/trips")]
[Authorize]
public class DriverTripController : ControllerBase
{
    private readonly IDriverTripService _service;
    private readonly ILogger<DriverTripController> _logger;

    public DriverTripController(IDriverTripService service, ILogger<DriverTripController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("today")]
    public async Task<IActionResult> GetTodayTrips()
    {
        try
        {
            if (!int.TryParse(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                out int driverId))
            {
                _logger.LogWarning("GetTodayTrips failed: Invalid DriverId claim");
                return Unauthorized();
            }

            if (!int.TryParse(
                User.FindFirst("companyId")?.Value,
                out int companyId))
            {
                _logger.LogWarning("GetTodayTrips failed: Invalid CompanyId claim");
                return Unauthorized();
            }

            var trips = await _service.GetTodayTrips(driverId, companyId);

            if (trips == null || !trips.Any())
            {
                _logger.LogInformation(
                    "No trips found for DriverId={DriverId}",
                    driverId);
            }

            _logger.LogInformation(
                "Today trips fetched successfully. DriverId={DriverId}",
                driverId);

            return Ok(ApiResponse<object>.Ok(
                new { trips },
                "Today's trips fetched successfully"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred while fetching today's trips. DriverId={DriverId}",
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            throw; 
        }
    }

    [HttpPost("{tripId:int}/start")]
    public async Task<IActionResult> StartTrip(int tripId)
    {
        int driverId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)
            ?.Value ?? throw new UnauthorizedAccessException("UserId claim missing"));
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var trip = await _service.StartTrip(tripId, driverId, companyId);

        if (trip == null)
        {
            _logger.LogWarning(
           "Trip starting failed. TripId={TripId}, DriverId={DriverId}",
           tripId,
           driverId);

            return BadRequest(ApiResponse<object>.Fail(
                StatusCodes.Status400BadRequest,
                "Trip cannot be started",
                new List<string> { "Trip may already be started or completed" }
            ));
        }

        return Ok(ApiResponse<object>.Ok(
            new { trip }, 
            "Trip started successfully"
        ));
    }

    [HttpPost("{tripId:int}/end")]
    public async Task<IActionResult> EndTrip(int tripId)
    {
        int driverId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)
            ?.Value ?? throw new UnauthorizedAccessException("UserId claim missing"));
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var trip = await _service.EndTrip(tripId, driverId, companyId);

        if (trip == null)
        {
            _logger.LogWarning(
            "Trip ending failed. TripId={TripId}, DriverId={DriverId}",
            tripId,
            driverId);

            return BadRequest(ApiResponse<object>.Fail(
                StatusCodes.Status400BadRequest,
                "Trip cannot be ended",
                new List<string> { "Trip not started or already completed" }
            ));
        }

        return Ok(ApiResponse<object>.Ok(
            new { trip },  
            "Trip ended successfully"
        ));
    }

    [HttpPost("{tripId:int}/location")]
    public async Task<IActionResult> LogLocation(int tripId,[FromBody] TripLocationLogRequest request)
    {
        int driverId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("UserId claim missing"));
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var result = await _service.LogTripLocation(tripId, driverId, companyId, request);

        if (result == null)
        {
            _logger.LogWarning(
              "Location logging failed. TripId={TripId}, DriverId={DriverId}",
              tripId,
              driverId);

            return BadRequest(ApiResponse<object>.Fail(
                StatusCodes.Status400BadRequest,
                "Location logging failed",
                new List<string> { "Trip not started or invalid" }
            ));
        }

        return Ok(ApiResponse<object>.Ok(
            new { location = result },
            "Location logged successfully"
        ));
    }

    [HttpPost("{tripId}/mark-attendance")]
    public async Task<IActionResult> MarkAttendance(int tripId,AttendanceRequestDto dto)
    {
        int driverId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("UserId claim missing"));
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var result = await _service.MarkAttendanceAsync(tripId, dto, driverId, companyId);

        return Ok(new
        {
            success = true,
            message = "Attendance marked successfully"
        });
    }


}
