using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    // GET: api/vehicle
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var vehicle = await _vehicleService.GetAllAsync(companyId);
        return Ok(vehicle);
    }

    // GET: api/vehicle/5
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var vehicle = await _vehicleService.GetByIdAsync(id,companyId);
        if (vehicle == null)
            return NotFound(new { message = "Vehicle not found" });

        return Ok(vehicle);
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] string? q)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var result = await _vehicleService.SearchAsync(q,companyId);
        return Ok(result);
    }

    // POST: api/vehicle
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(Vehicle vehicle)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        vehicle.CompanyId = companyId;

        var created = await _vehicleService.CreateAsync(vehicle);

        return CreatedAtAction(nameof(GetById), new { id = created.VehicleId }, created);
    }

    // PUT: api/vehicle/5
    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id,Vehicle vehicle)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        vehicle.CompanyId = companyId;

        var updated = await _vehicleService.UpdateAsync(id, vehicle,companyId);

        if (updated == null)
            return NotFound(new { message = "Vehicle not found" });

        return Ok(updated);
    }

    // DELETE: api/vehicle/5
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var success = await _vehicleService.DeleteAsync(id,companyId);

        if (!success)
            return NotFound(new { message = "Vehicle not found" });

        return Ok(new { message = "Vehicle deleted successfully" });
    }
    [HttpPost("upload-photo")]
    public async Task<IActionResult> UploadPhoto(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest();

        var folder = Path.Combine("wwwroot/uploads/vehicles");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var path = Path.Combine(folder, fileName);

        using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        return Ok(new
        {
            photoUrl = $"{baseUrl}/uploads/vehicles/{fileName}"
        });

    }
}
