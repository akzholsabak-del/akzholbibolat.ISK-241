using System.Security.Cryptography;

namespace ViewModelValidation.Services;

/// <summary>Құпиясөзді PBKDF2 арқылы хэштеу (ашық түрде сақтауға болмайды).</summary>
public static class PasswordHelper
{
    public static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
}
