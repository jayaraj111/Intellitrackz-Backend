namespace AdminDashboard.DtoModels;

public class BulkAttendanceDto
{
    public int TripId { get; set; }
    public int MarkedBy { get; set; }
    public List<PassengerAttendanceDto> Passengers { get; set; }
}
