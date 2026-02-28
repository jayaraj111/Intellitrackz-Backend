using AdminDashboard.Decryption;
using AdminDashboard.DtoModels;
using AdminDashboard.Models;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    // GET: api/user
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var user = await _userService.GetAllAsync();
        return Ok(user);
    }

    [HttpGet("passengers")]
    [Authorize]
    public async Task<IActionResult> GetPassengers()
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var passengers = await _userService.GetPassengersAsync(companyId);
        return Ok(passengers);
    }

    [HttpGet("drivers")]
    [Authorize]
    public async Task<IActionResult> GetDrivers()
    {
        int companyId = int.Parse(User.FindFirst("companyId")!.Value);
        var passengers = await _userService.GetDriversAsync(companyId);
        return Ok(passengers);
    }


    // GET: api/user/5
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
    }

    [HttpGet("user-details/{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetUserDetailsById(int id)
    {
        var user = await _userService.GetUserDetailsByIdAsync(id);
        if (user == null) return NotFound();

        return Ok(new
        {
            userId = user.UserId,
            username = user.Username,
            fullName = user.FullName,
            email = user.Email,
            phoneNumber = user.PhoneNumber,
            userType = user.UserType,
            photoUrl = !string.IsNullOrEmpty(user.PhotoUrl)? $"{Request.Scheme}://{Request.Host}{user.PhotoUrl}" : null,
            dateOfBirth = user.DateOfBirth,
            companyName = user.Company?.CompanyName 
        });
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] string? q)
    {
        var result = await _userService.SearchAsync(q);
        return Ok(result);
    }

    // POST: api/user
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _userService.CreateAsync(user);

        return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
    }

    // PUT: api/user/5
    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id,User user)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _userService.UpdateAsync(id, user);

        if (updated == null)
            return NotFound(new { message = "User not found" });

        return Ok(updated);
    }

    // DELETE: api/user/5
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _userService.DeleteAsync(id);

        if (!success)
            return NotFound(new { message = "User not found" });

        return Ok(new { message = "User deleted successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userService.GetByUsernameAsync(request.Username);

        if (user == null || user.Status != 'Y')
        {
            return Unauthorized(ApiResponse<LoginData>.Fail(
                StatusCodes.Status401Unauthorized,
                "Invalid username or password"
            ));
        }

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(ApiResponse<LoginData>.Fail(
                StatusCodes.Status401Unauthorized,
                "Invalid username or password"
            ));
        }

        var token = GenerateJwtToken(user);

        var loginResponse = new LoginResponse
        {
            Token = token,
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            UserType = user.UserType,
            CompanyId = user.CompanyId,
            IsFirstLogin = user.IsFirstLogin,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            CompanyName = user.Company?.CompanyName,
            PhotoUrl = !string.IsNullOrEmpty(user.PhotoUrl) ? $"{Request.Scheme}://{Request.Host}{user.PhotoUrl}" : null,
            DateOfBirth = user.DateOfBirth
        };

        var data = new LoginData
        {
            LoginResponse = loginResponse
        };

        return Ok(ApiResponse<LoginData>.Ok(
            data,
            "Login successful"
        ));
    }


    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var user = await _userService.GetByIdAsync(request.UserId);
        if (user == null) return NotFound();

        user.PasswordHash = request.NewPassword;
        user.IsFirstLogin = 'N';
        user.UpdatedAt = DateTime.UtcNow;
        await _userService.UpdateAsync(user.UserId, user);

        return Ok(new { message = "Password changed successfully" });
    }

    [HttpPut("update-profile/{id}")]
    public async Task<IActionResult> UpdateProfile(int id, [FromForm] ProfileUpdateDto model)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        user.FullName = model.FullName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.DateOfBirth = model.DateOfBirth;

        if (model.Photo != null && model.Photo.Length > 0)
        {
            if (!string.IsNullOrEmpty(user.PhotoUrl))
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.PhotoUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
            }

            var fileName = $"profile_{id}_{DateTime.UtcNow.Ticks}{Path.GetExtension(model.Photo.FileName)}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/profiles", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

           
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Photo.CopyToAsync(stream);
            }

            user.PhotoUrl = $"/uploads/profiles/{fileName}";
        }

        try
        {
            await _userService.UpdateAsync(id, user);
            return Ok(new { message = "Profile updated successfully", photoUrl = user.PhotoUrl });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim("fullName", user.FullName),
            new Claim("userType", user.UserType ?? "User"),
            new Claim("companyId", user.CompanyId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
