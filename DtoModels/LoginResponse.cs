namespace AdminDashboard.DtoModels;

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? UserType { get; set; }
    public int CompanyId { get; set; }
    public char IsFirstLogin { get; set; }
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? CompanyName { get; set; }
}
