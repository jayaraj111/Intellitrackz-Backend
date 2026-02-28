namespace AdminDashboard.Models;
public class User
{
    public int UserId { get; set; }
    public int CompanyId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? UserType { get; set; }
    public string? Remarks { get; set; }
    public char Status { get; set; } = 'Y';
    public char IsFirstLogin { get; set; } = 'Y';
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DateOfBirth { get; set; } 
    public string? PhotoUrl { get; set; }
    public Company? Company { get; set; }
}
