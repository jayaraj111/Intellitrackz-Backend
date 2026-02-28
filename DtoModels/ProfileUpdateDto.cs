namespace AdminDashboard.DtoModels;

public class ProfileUpdateDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public IFormFile? Photo { get; set; }
}