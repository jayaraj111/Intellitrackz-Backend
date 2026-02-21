using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.DtoModels;

public class PassengerAllocationDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int RouteId { get; set; }

    [Required]
    public int StopId { get; set; }
}
