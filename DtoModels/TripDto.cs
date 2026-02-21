namespace AdminDashboard.DtoModels;

public class TripDto
{
    public int? TripId { get; set; }
    public int RouteId { get; set; }
    public int VehicleId { get; set; }
    public int DriverId { get; set; }
    public DateTime TripDate { get; set; }
    public string SessionName { get; set; } = null!;
    public TimeSpan? PlannedStartTime { get; set; }
    public TimeSpan? PlannedEndTime { get; set; }
    public char Status { get; set; } = 'P';
}
