using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace AdminDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RouteStopController : ControllerBase
{
    private readonly IRouteStopService _routeStopService;

    public RouteStopController(IRouteStopService routeStopService)
    {
        _routeStopService = routeStopService;
    }

    private int GetCompanyId() => int.Parse(User.FindFirst("companyId")!.Value);

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var stops = await _routeStopService.GetAllAsync(GetCompanyId());
        return Ok(stops);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var stop = await _routeStopService.GetByIdAsync(id, GetCompanyId());
        if (stop == null) return NotFound(new { message = "Stop not found" });
        return Ok(stop);
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] string? q)
    {
        var result = await _routeStopService.SearchAsync(q, GetCompanyId());
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(Stop stop)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        stop.CompanyId = GetCompanyId();
        var created = await _routeStopService.CreateAsync(stop);

        return CreatedAtAction(nameof(GetById), new { id = created.StopId }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, Stop stop)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _routeStopService.UpdateAsync(id, stop, GetCompanyId());
        if (updated == null) return NotFound(new { message = "Stop not found" });

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _routeStopService.DeleteAsync(id, GetCompanyId());
        if (!success) return NotFound(new { message = "Stop not found" });

        return Ok(new { message = "Stop deleted successfully" });
    }

    [HttpGet("exists")]
    public async Task<IActionResult> StopExists(string name)
    {
        int companyId = GetCompanyId();

        bool exists =
            await _routeStopService.StopExistsAsync(name, companyId);

        return Ok(new { exists });
    }



}
