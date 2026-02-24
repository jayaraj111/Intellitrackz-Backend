using AdminDashboard.DtoModels;
using AdminDashboard.Services;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;

[ApiController]
[Route("api/trips")]
[Authorize(Policy = "AdminOnly")]
public class TripController : ControllerBase
{
    private readonly ITripService _service;

    public TripController(ITripService service)
    {
        _service = service;
    }

    // 🔹 GET: api/trips
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        return Ok(await _service.GetAllAsync(companyId));
    }

    // 🔹 GET: api/trips/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var trip = await _service.GetByIdAsync(id, companyId);
        return trip == null ? NotFound() : Ok(trip);
    }

    // 🔹 SEARCH: api/trips/search?q=morning
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? q)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        return Ok(await _service.SearchAsync(companyId, q));
    }

    // 🔹 POST: api/trips
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TripDto dto)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var created = await _service.CreateAsync(dto, companyId);
        return Ok(created);
    }

    // 🔹 PUT: api/trips/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TripDto dto)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var updated = await _service.UpdateAsync(id, dto, companyId);
        return updated == null ? NotFound() : Ok(updated);
    }

    // 🔹 DELETE: api/trips/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var success = await _service.DeleteAsync(id, companyId);
        return success ? Ok() : NotFound();
    }

    [HttpPost("generate-from-schedule")]
    public async Task<IActionResult> GenerateTrips(int scheduleId,DateTime startDate,DateTime endDate)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var count = await _service.GenerateFromScheduleAsync(scheduleId,startDate,endDate,companyId);

        return Ok(new
        {
            message = $"{count} trips generated successfully"
        });
    }

    [HttpGet("{tripId}/route-path")]
    public async Task<IActionResult> GetTripPath(int tripId)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var data = await _service.GetTripPathAsync(tripId, companyId);
        return Ok(data);
    }


}
