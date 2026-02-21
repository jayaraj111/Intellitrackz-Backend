namespace AdminDashboard.DtoModels;

public class RouteStopMappingDetailDto
{
    public int RouteId { get; set; }
    public List<RouteStopItemDto> Stops { get; set; } = new();
}
