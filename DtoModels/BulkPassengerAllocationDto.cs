namespace AdminDashboard.DtoModels;

public class BulkPassengerAllocationDto
{
    public int RouteId { get; set; }
    public List<PassengerAllocationItemDto> Allocations { get; set; } = new();
}
