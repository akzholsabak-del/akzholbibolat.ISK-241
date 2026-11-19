namespace ViewModelValidation.Models;

/// <summary>
/// Domain Model – пайдаланушы. PasswordHash сақталады, ал ашық құпиясөз
/// (Password, ConfirmPassword) тек ViewModel-де болады.
/// </summary>
public class AppUser
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public int Course { get; set; }
    public string? Bio { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
}
