namespace AdminDashboard.DtoModels;

public class PassengerDto
{
    public int PassengerId { get; set; }
    public string PassengerName { get; set; } = string.Empty;
    public bool? IsBoarded { get; set; }
}
