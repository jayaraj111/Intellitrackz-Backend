namespace AdminDashboard.DtoModels;

public class StopWithPassengersDto
{
    public int StopId { get; set; }
    public string StopName { get; set; } = string.Empty;
    public string Latitude { get; set; } = string.Empty;
    public string Longitude { get; set; } = string.Empty;
    public TimeSpan PlannedArrivalTime { get; set; }
    public List<PassengerDto> Passengers { get; set; } = new();
}
