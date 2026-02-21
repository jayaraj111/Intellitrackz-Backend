using AdminDashboard.Models;
using Microsoft.AspNetCore.Identity;

namespace AdminDashboard.Decryption;

public static class PasswordHasher
{
    private static readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

    public static string Hash(string plainPassword, User user = null!)
    {
        return _hasher.HashPassword(user ?? new User(), plainPassword);
    }

    //public static bool Verify(string hashedPassword, string plainPassword, User user = null!)
    //{
    //    var result = _hasher.VerifyHashedPassword(user ?? new User(), hashedPassword, plainPassword);
    //    return result == PasswordVerificationResult.Success ||
    //           result == PasswordVerificationResult.SuccessRehashNeeded;
    //}

    public static bool Verify(string plainPassword, string storedHash)
    {
        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(new User(), storedHash, plainPassword);

        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }


}
