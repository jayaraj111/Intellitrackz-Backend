namespace AdminDashboard.Models;

public class TripLocationLog
{
    public long TripLocationLogId { get; set; }
    public int TripId { get; set; }
    public int CompanyId { get; set; }
    public int DriverId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime LoggedAt { get; set; } = DateTime.Now;
    public Trip? Trip { get; set; }
}
