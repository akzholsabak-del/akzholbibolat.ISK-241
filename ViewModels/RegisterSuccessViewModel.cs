namespace ViewModelValidation.ViewModels;

/// <summary>Тіркеуден кейін көрсетілетін деректер (PasswordHash-сіз).</summary>
public class RegisterSuccessViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public int Course { get; set; }
    public DateTime RegisteredAt { get; set; }
}
