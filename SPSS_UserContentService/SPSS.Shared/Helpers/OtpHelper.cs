using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.Helpers;

public static class OtpHelper
{
    public static string GenerateNumericOtp(int length = 6)
    {
        var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        var sb = new StringBuilder(length);
        for (int i = 0; i < length; i++)
            sb.Append((bytes[i] % 10).ToString());
        return sb.ToString();
    }

    public static string HashOtp(string otp, string key, string salt)
    {
        // HMACSHA256(key) of (salt + otp)
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        var input = Encoding.UTF8.GetBytes(salt + otp);
        var hash = hmac.ComputeHash(input);
        return Convert.ToBase64String(hash);
    }

    public static string CreateSalt(int size = 16)
    {
        var bytes = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
