using AdminDashboard.Data;
using AdminDashboard.Decryption;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<IEnumerable<User>> GetPassengersAsync(int companyId)
    {
        return await _context.Users
            .Where(u =>
                u.CompanyId == companyId &&
                u.UserType == "Passenger" &&
                u.Status == 'Y')
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetDriversAsync(int companyId)
    {
        return await _context.Users
            .Where(u =>
                u.CompanyId == companyId &&
                u.UserType == "Driver" &&
                u.Status == 'Y')
            .ToListAsync();
    }


    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetUserDetailsByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> SearchAsync(string? keyword)
    {
        keyword = keyword?.Trim().ToLower();

        return await _context.Users
            .Where(u =>
                keyword == null ||
                u.FullName.ToLower().Contains(keyword) ||
                u.Email.ToLower().Contains(keyword) ||
                u.PhoneNumber.ToLower().Contains(keyword) ||
                u.UserType.ToLower().Contains(keyword)
            )
            .ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            throw new Exception("Username already exists.");

        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            throw new Exception("Email already exists.");

        user.PasswordHash = PasswordHasher.Hash(user.PasswordHash);
        user.CreatedAt = DateTime.UtcNow;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> UpdateAsync(int id, User user)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

        if (existing == null)
            return null;

        if (existing.Username != user.Username)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                throw new Exception("Username already taken.");

            existing.Username = user.Username; 
        }

        existing.FullName = user.FullName;
        existing.Email = user.Email;
        existing.PhoneNumber = user.PhoneNumber;
        existing.Status = user.Status;
        existing.UserType = user.UserType;
        existing.CompanyId = user.CompanyId;
        existing.Remarks = user.Remarks;
        existing.UpdatedAt = DateTime.UtcNow;
        existing.DateOfBirth = user.DateOfBirth;
        existing.PhotoUrl = user.PhotoUrl;

        if (!string.IsNullOrWhiteSpace(user.PasswordHash) && existing.PasswordHash != user.PasswordHash )
        {
            existing.PasswordHash = PasswordHasher.Hash(user.PasswordHash);
            existing.IsFirstLogin = 'N';
        }
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Users.FindAsync(id);

        if (existing == null)
            return false;

        _context.Users.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
