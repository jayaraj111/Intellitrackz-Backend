using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    // GET: api/companies
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var companies = await _companyService.GetAllAsync();
        return Ok(companies);
    }

    // GET: api/companies/5
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var company = await _companyService.GetByIdAsync(id);
        if (company == null)
            return NotFound(new { message = "Company not found" });

        return Ok(company);
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] string? q)
    {
        var result = await _companyService.SearchAsync(q);
        return Ok(result);
    }

    // POST: api/companies
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(Company company)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _companyService.CreateAsync(company);

        return CreatedAtAction(nameof(GetById), new { id = created.CompanyId }, created);
    }

    // PUT: api/companies/5
    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id,Company company)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _companyService.UpdateAsync(id, company);

        if (updated == null)
            return NotFound(new { message = "Company not found" });

        return Ok(updated);
    }

    // DELETE: api/companies/5
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _companyService.DeleteAsync(id);

        if (!success)
            return NotFound(new { message = "Company not found" });

        return Ok(new { message = "Company deleted successfully" });
    }
}