namespace AdminDashboard.DtoModels;

public class RouteDto
{
    public int RouteId { get; set; }
    public string RouteName { get; set; } = string.Empty;
    public string? StartLocationLat { get; set; }
    public string? StartLocationLng { get; set; }
    public string? EndLocationLat { get; set; }
    public string? EndLocationLng { get; set; }
}
