namespace AdminDashboard.DtoModels;

public class VehicleDto
{
    public int VehicleId { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string? ImeiId { get; set; }
    public string? PhotoUrl { get; set; }
}
