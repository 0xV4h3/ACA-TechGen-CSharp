namespace WelcomePage.Services;

public class BcryptPasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor: 12);
    }

    public bool VerifyPassword(string providedPassword, string storedHash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, storedHash);
    }
}