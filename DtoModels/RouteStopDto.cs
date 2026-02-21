using AdminDashboard.Models;

namespace AdminDashboard.DtoModels;

public class RouteStopDto
{
    public int RouteStopId { get; set; }
    public string StopName { get; set; } = null!;
    public string? RouteName { get; set; } 
    public int StopOrder { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public TimeSpan PlannedArrivalTime { get; set; }
}
