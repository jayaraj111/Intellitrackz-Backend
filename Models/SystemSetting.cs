using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Models;

public class SystemSetting
{
    [Key]
    public int Id { get; set; }
    public int? CompanyId { get; set; }
    [Required]
    [MaxLength(100)]
    public string SettingKey { get; set; } = null!;
    [Required]
    [MaxLength(500)]
    public string SettingValue { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public Company? Company { get; set; }
}
