namespace AdminDashboard.DtoModels;

public class RouteStopItemDto
{
    public int RouteStopId { get; set; }
    public int StopId { get; set; }
    public string? StopName { get; set; }
    public int StopOrder { get; set; }
    public string PlannedArrivalTime { get; set; } = "00:00";
}