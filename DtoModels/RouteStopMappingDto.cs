namespace AdminDashboard.DtoModels;

public class RouteStopMappingDto
{
    public int RouteId { get; set; }
    public List<StopSelectionDto> Stops { get; set; } = new();
}
