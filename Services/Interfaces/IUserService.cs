using AdminDashboard.Models;

namespace AdminDashboard.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<IEnumerable<User>> GetPassengersAsync(int companyId);
    Task<IEnumerable<User>> GetDriversAsync(int companyId);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetUserDetailsByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<IEnumerable<User>> SearchAsync(string? keyword);
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(int id, User user);
    Task<bool> DeleteAsync(int id);
}
