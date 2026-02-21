namespace AdminDashboard.Models;

public class TripAttendance
{
    public int TripAttendanceId { get; set; }
    public int CompanyId { get; set; }
    public int TripId { get; set; }
    public int PassengerId { get; set; }
    public int StopId { get; set; }
    public bool IsBoarded { get; set; }
    public DateTime MarkedAt { get; set; }
    public int MarkedBy { get; set; } 
    public Trip Trip { get; set; }
}