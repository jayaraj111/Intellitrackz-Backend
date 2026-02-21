namespace AdminDashboard.Models;

public class RoutePassenger
{
    public int RoutePassengerId { get; set; }

    public int CompanyId { get; set; }
    public int UserId { get; set; }      // Passenger
    public int RouteId { get; set; }
    public int StopId { get; set; }      // MASTER StopId
    public char Status { get; set; } = 'Y';
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Company? Company { get; set; }
    public User? User { get; set; }
    public TransportRoute? Route { get; set; }
    public Stop? Stop { get; set; }
}
