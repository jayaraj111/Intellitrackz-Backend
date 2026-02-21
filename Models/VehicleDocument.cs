using System.Text.Json.Serialization;

namespace AdminDashboard.Models;

public class VehicleDocument
{
    public int VehicleDocumentId { get; set; }
    public int VehicleId { get; set; }
    [JsonIgnore]
    public Vehicle? Vehicle { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string? PhotoUrl { get; set; }
}





