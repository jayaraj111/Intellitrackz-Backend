namespace AdminDashboard.DtoModels;

public class StopAttendanceDto
{
    public int StopId { get; set; }
    public List<PassengerAttendanceDto> Passengers { get; set; }
}