namespace AdminDashboard.DtoModels;

public class StopSelectionDto
{
    public int StopId { get; set; }
    public int StopOrder { get; set; }
    public string PlannedArrivalTime { get; set; } = "00:00";
}
