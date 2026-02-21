namespace AdminDashboard.DtoModels;

public class TripDetailsDto
{
    public int TripId { get; set; }
    public DateTime TripDate { get; set; }
    public string Session { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public DateTime? ActualStartTime { get; set; }
    public DateTime? ActualEndTime { get; set; }
}
