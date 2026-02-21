namespace AdminDashboard.Models;

public class Vehicle
{
    public int VehicleId { get; set; }
    public int CompanyId { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string? Model { get; set; }
    public string? VehiclePhotoUrl { get; set; }
    public string? ImeiId { get; set; }
    public string? Remarks { get; set; }
    public char Status { get; set; } = 'Y';
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Company? Company { get; set; }
    public ICollection<VehicleDocument>? Documents { get; set; }
}





