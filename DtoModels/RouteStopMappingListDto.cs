namespace AdminDashboard.DtoModels;

public class RouteStopMappingListDto
{
    public int RouteId { get; set; }
    public string RouteName { get; set; } = null!;
    public int TotalStops { get; set; }
    public string FirstStop { get; set; } = null!;
    public string LastStop { get; set; } = null!;
}
