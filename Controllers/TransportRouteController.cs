using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransportRouteController : ControllerBase
{
    private readonly ITransportRouteService _transportRouteService;

    public TransportRouteController(ITransportRouteService transportRouteService)
    {
        _transportRouteService = transportRouteService;
    }

    private int GetCompanyId() => int.Parse(User.FindFirst("companyId")!.Value);

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var transportRoute = await _transportRouteService.GetAllAsync(GetCompanyId());
        return Ok(transportRoute);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var transportRoute = await _transportRouteService.GetByIdAsync(id, GetCompanyId());
        if (transportRoute == null)
            return NotFound(new { message = "TransportRoute not found" });

        return Ok(transportRoute);
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] string? q)
    {
        var result = await _transportRouteService.SearchAsync(q, GetCompanyId());
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(TransportRoute transportRoute)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        transportRoute.CompanyId = GetCompanyId();
        var created = await _transportRouteService.CreateAsync(transportRoute);

        return CreatedAtAction(nameof(GetById), new { id = created.RouteId }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, TransportRoute transportRoute)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _transportRouteService.UpdateAsync(id, transportRoute, GetCompanyId());

        if (updated == null)
            return NotFound(new { message = "TransportRoute not found" });

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _transportRouteService.DeleteAsync(id, GetCompanyId());

        if (!success)
            return NotFound(new { message = "TransportRoute not found" });

        return Ok(new { message = "TransportRoute deleted successfully" });
    }
}
