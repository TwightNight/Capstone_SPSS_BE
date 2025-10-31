using BCrypt.Net; // <-- THÊM DÒNG NÀY
using System;

namespace SPSS.Shared.Helpers;

public class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch (Exception)
        {
            return false;
        }
    }
}