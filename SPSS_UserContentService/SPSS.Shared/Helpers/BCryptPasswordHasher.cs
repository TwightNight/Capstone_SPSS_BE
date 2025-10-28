using BCrypt.Net; // <-- THÊM DÒNG NÀY
using System;

namespace SPSS.Shared.Helpers;

public class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        // Fix: Use BCrypt.Net.BCrypt instead of BCrypt
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        try
        {
            // Fix: Use BCrypt.Net.BCrypt instead of BCrypt
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch (Exception)
        {
            return false;
        }
    }
}