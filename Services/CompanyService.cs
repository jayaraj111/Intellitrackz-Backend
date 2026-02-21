using AdminDashboard.Data;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Services;

public class CompanyService : ICompanyService
{
    private readonly AppDbContext _context;

    public CompanyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies.ToListAsync();
    }

    public async Task<Company?> GetByIdAsync(int id)
    {
        return await _context.Companies.FindAsync(id);
    }

    public async Task<IEnumerable<Company>> SearchAsync(string? keyword)
    {
        keyword = keyword?.Trim().ToLower();

        return await _context.Companies
            .Where(c =>
                keyword == null ||
                c.CompanyName.ToLower().Contains(keyword) ||
                c.Address.ToLower().Contains(keyword) ||
                c.Email.ToLower().Contains(keyword) ||
                c.ContactNumber.ToLower().Contains(keyword)
            )
            .ToListAsync();
    }

    public async Task<Company> CreateAsync(Company company)
    {
        company.CreatedAt = DateTime.UtcNow;

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<Company?> UpdateAsync(int id, Company company)
    {
        var existing = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == id);

        if (existing == null)
            return null;

        // Update fields
        existing.CompanyName = company.CompanyName;
        existing.Address = company.Address;
        existing.ContactNumber = company.ContactNumber;
        existing.Email = company.Email;
        existing.Status = company.Status;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Companies.FindAsync(id);

        if (existing == null)
            return false;

        _context.Companies.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
