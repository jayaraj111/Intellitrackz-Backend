using AdminDashboard.Models;

namespace AdminDashboard.Services.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(int id);
    Task<IEnumerable<Company>> SearchAsync(string? keyword);
    Task<Company> CreateAsync(Company company);
    Task<Company?> UpdateAsync(int id, Company company);
    Task<bool> DeleteAsync(int id);
}
