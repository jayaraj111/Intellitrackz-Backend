namespace AdminDashboard.Models;

public class RouteStop
{
    public int RouteStopId { get; set; }
    public int RouteId { get; set; }
    public int StopId { get; set; }
    public int StopOrder { get; set; }
    public TransportRoute? Route { get; set; }
    public Stop? Stop { get; set; }
    public TimeSpan PlannedArrivalTime { get; set; }
}


