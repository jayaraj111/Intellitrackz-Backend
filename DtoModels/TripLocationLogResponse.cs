namespace AdminDashboard.DtoModels;

public class TripLocationLogResponse
{
    public long Id { get; set; }
    public int TripId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime LoggedAt { get; set; }
}
