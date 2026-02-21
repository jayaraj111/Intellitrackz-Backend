namespace AdminDashboard.Models;

public class TripSchedule
{
    public int TripScheduleId { get; set; }
    public string TripName { get; set; } = string.Empty;
    public DateTime TripStartDate { get; set; }
    public DateTime? TripEndDate { get; set; }
    public TimeSpan PlannedStartTime { get; set; }
    public TimeSpan? PlannedEndTime { get; set; }
    public int RouteId { get; set; }
    public TransportRoute? Route { get; set; }
    public bool Mon { get; set; }
    public bool Tue { get; set; }
    public bool Wed { get; set; }
    public bool Thu { get; set; }
    public bool Fri { get; set; }
    public bool Sat { get; set; }
    public bool Sun { get; set; }
    public char Status { get; set; } = 'Y';
}