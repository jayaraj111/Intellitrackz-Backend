namespace AdminDashboard.Models;

public class TransportRoute
{
    public int RouteId { get; set; }           
    public int CompanyId { get; set; }
    public string RouteName { get; set; } = null!;
    public double? StartLocationLat { get; set; }
    public double? StartLocationLng { get; set; }
    public double? EndLocationLat { get; set; }
    public double? EndLocationLng { get; set; }
    public string? Remarks { get; set; }
    public char Status { get; set; } = 'Y';
    public DateTime CreatedAt { get; set; }
    public Company? Company { get; set; }                 
    public List<RouteStop> RouteStops { get; set; } = new List<RouteStop>();
}

