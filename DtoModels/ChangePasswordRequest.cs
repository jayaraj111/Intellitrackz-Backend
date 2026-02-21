namespace AdminDashboard.DtoModels;

public class ChangePasswordRequest
{
    public int UserId { get; set; } 
    public string NewPassword { get; set; } = null!;
}
