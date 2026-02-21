namespace AdminDashboard.DtoModels;

public class TripScheduleDto
{
    public string TripName { get; set; } = "";
    public DateTime TripStartDate { get; set; }
    public DateTime? TripEndDate { get; set; }
    public string PlannedStartTime { get; set; } = "06:00";
    public string? PlannedEndTime { get; set; }
    public int RouteId { get; set; }
    public bool Mon { get; set; }
    public bool Tue { get; set; }
    public bool Wed { get; set; }
    public bool Thu { get; set; }
    public bool Fri { get; set; }
    public bool Sat { get; set; }
    public bool Sun { get; set; }
}
