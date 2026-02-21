using AdminDashboard.DtoModels;
using AdminDashboard.Services;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteStopMappingController : ControllerBase
    {
        private readonly IRouteStopMappingService _routeStopMappingService;

        public RouteStopMappingController(IRouteStopMappingService routeStopMappingService)
        {
            _routeStopMappingService = routeStopMappingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _routeStopMappingService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{routeId:int}")]
        public async Task<IActionResult> GetByRoute(int routeId)
        {
            var result = await _routeStopMappingService.GetByRouteIdAsync(routeId);
            if (result == null)
                return NotFound(new { message = "Mapping not found" });

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? q)
        {
            return Ok(await _routeStopMappingService.SearchAsync(q));
        }


        [HttpPost("save-mapping")]
        public async Task<IActionResult> SaveMapping([FromBody] RouteStopMappingDto mappingDto)
        {
            if (mappingDto == null || mappingDto.RouteId <= 0)
            {
                return BadRequest("Invalid route mapping data.");
            }

            var result = await _routeStopMappingService.SaveRouteStopsMappingAsync(mappingDto);

            if (result)
            {
                return Ok(new { message = "Route stops mapped successfully." });
            }

            return StatusCode(500, "An error occurred while saving route stops.");
        }

        [HttpDelete("{routeId:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int routeId)
        {
            var success = await _routeStopMappingService.DeleteByRouteIdAsync(routeId);

            if (!success)
                return NotFound(new { message = "No mapping found to delete" });

            return Ok(new { message = "Route mapping deleted successfully" });
        }

    }
}

