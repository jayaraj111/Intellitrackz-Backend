namespace AdminDashboard.DtoModels;

public class TodayTripResponse
{
    public int TripId { get; set; }
    public DateTime TripDate { get; set; }
    public string Session { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public RouteDto Route { get; set; } = null!;
    public VehicleDto Vehicle { get; set; } = null!;
    public List<StopWithPassengersDto> Stops { get; set; } = new();
}
