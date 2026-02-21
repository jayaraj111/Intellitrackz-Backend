namespace AdminDashboard.Models;

public class Stop
{
    public int StopId { get; set; }
    public int CompanyId { get; set; }
    public string StopName { get; set; } = null!;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public char Status { get; set; } = 'Y';
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Company? Company { get; set; }
}

