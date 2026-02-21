using AdminDashboard.DtoModels;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;

[ApiController]
[Route("api/passenger-allocation")]
[Authorize]
public class PassengerAllocationController : ControllerBase
{
    private readonly IPassengerAllocationService _service;

    public PassengerAllocationController(IPassengerAllocationService service)
    {
        _service = service;
    }

    // 🔹 GET: api/passenger-allocation
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var result = await _service.GetAllAsync(companyId);
        return Ok(result);
    }

    // 🔹 GET: api/passenger-allocation/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var allocation = await _service.GetByIdAsync(id, companyId);
        if (allocation == null)
            return NotFound(new { message = "Passenger allocation not found" });

        return Ok(allocation);
    }

    [HttpGet("route/{routeId:int}")]
    public async Task<IActionResult> GetByRoute(int routeId)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var allocations = await _service.GetByRouteIdAllocationsAsync(routeId, companyId);
        return Ok(allocations);
    }

    // 🔹 GET: api/passenger-allocation/search?q=abc
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? q)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var result = await _service.SearchAsync(companyId, q);
        return Ok(result);
    }



    [HttpPost("sync")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> SyncAllocations([FromBody] BulkPassengerAllocationDto dto)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        if (dto.RouteId <= 0 || dto.Allocations == null)
            return BadRequest("Invalid route or empty allocations.");

        var success = await _service.SyncRouteAllocationsAsync(dto, companyId);

        if (!success)
            return StatusCode(500, "An error occurred while saving allocations.");

        return Ok(new { message = "Allocations synchronized successfully" });
    }


    // 🔹 PUT: api/passenger-allocation/5
    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] PassengerAllocationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var updated = await _service.UpdateAsync(id, dto, companyId);
        if (updated == null)
            return NotFound(new { message = "Passenger allocation not found" });

        return Ok(updated);
    }



    // 🔹 DELETE: api/passenger-allocation/5
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var success = await _service.DeleteAsync(id, companyId);
        if (!success)
            return NotFound(new { message = "Passenger allocation not found" });

        return Ok(new { message = "Passenger allocation deleted successfully" });
    }
}

