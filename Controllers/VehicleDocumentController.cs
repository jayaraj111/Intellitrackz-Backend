using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;

[ApiController]
[Route("api/vehicle-documents")]
[Authorize]
public class VehicleDocumentController : ControllerBase
{
    private readonly IVehicleDocumentService _service;

    public VehicleDocumentController(IVehicleDocumentService service)
    {
        _service = service;
    }

    [HttpGet("{vehicleId:int}")]
    public async Task<IActionResult> GetByVehicle(int vehicleId)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var docs = await _service.GetByVehicleAsync(vehicleId, companyId);
        return Ok(docs);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(VehicleDocument doc)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var result = await _service.CreateAsync(doc, companyId);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var success = await _service.DeleteAsync(id, companyId);

        if (!success) return NotFound();

        return Ok();
    }
}
