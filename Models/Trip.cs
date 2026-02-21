using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDashboard.Models;


[Table("Trip")]
public class Trip
{
    public int TripId { get; set; }
    public int CompanyId { get; set; }
    public int RouteId { get; set; }
    public int? VehicleId { get; set; }
    public int? DriverId { get; set; }
    public DateTime TripDate { get; set; }
    public string SessionName { get; set; } = null!;
    public TimeSpan? PlannedStartTime { get; set; }
    public TimeSpan? PlannedEndTime { get; set; }
    public DateTime? ActualStartTime { get; set; }
    public DateTime? ActualEndTime { get; set; }
    public char Status { get; set; } = 'P';
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public TransportRoute? Route { get; set; }
    public Vehicle? Vehicle { get; set; }
    public User? Driver { get; set; }
}
