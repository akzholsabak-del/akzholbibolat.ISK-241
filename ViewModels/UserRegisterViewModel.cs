using System.ComponentModel.DataAnnotations;
using ViewModelValidation.Validation;

namespace ViewModelValidation.ViewModels;

/// <summary>
/// Пайдаланушы профилін тіркеуге арналған ViewModel.
/// Password және ConfirmPassword тек осы моделде болады – AppUser-де тек PasswordHash сақталады.
/// </summary>
public class UserRegisterViewModel
{
    [Display(Name = "Аты-жөні")]
    [Required(ErrorMessage = "{0} міндетті түрде толтырылуы керек")]
    [StringLength(60, MinimumLength = 2,
        ErrorMessage = "{0} ұзындығы {2} мен {1} таңба аралығында болуы керек")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Логин")]
    [Required(ErrorMessage = "{0} міндетті түрде толтырылуы керек")]
    [StringLength(20, MinimumLength = 3,
        ErrorMessage = "{0} ұзындығы {2} мен {1} таңба аралығында болуы керек")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$",
        ErrorMessage = "{0} тек латын әріптерінен, цифрлардан және '_' белгісінен тұруы керек")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "{0} міндетті түрде толтырылуы керек")]
    [EmailAddress(ErrorMessage = "{0} пішімі дұрыс емес")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Телефон")]
    [Required(ErrorMessage = "{0} міндетті түрде толтырылуы керек")]
    [RegularExpression(@"^\+7\d{10}$",
        ErrorMessage = "{0} пішімі: +7XXXXXXXXXX (мысалы, +77011234567)")]
    public string Phone { get; set; } = string.Empty;

    [Display(Name = "Туған күні")]
    [Required(ErrorMessage = "{0} көрсетілуі керек")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [MinimumAge(16)]
    public DateTime? BirthDate { get; set; }

    [Display(Name = "Курс")]
    [Required(ErrorMessage = "{0} көрсетілуі керек")]
    [Range(1, 6, ErrorMessage = "{0} {1} мен {2} аралығында болуы керек")]
    public int? Course { get; set; }

    [Display(Name = "Өзіңіз туралы")]
    [StringLength(300, ErrorMessage = "{0} {1} таңбадан аспауы керек")]
    [NoForbiddenWords]
    public string? Bio { get; set; }

    [Display(Name = "Құпиясөз")]
    [Required(ErrorMessage = "{0} міндетті түрде толтырылуы керек")]
    [StringLength(50, MinimumLength = 8,
        ErrorMessage = "{0} ұзындығы {2} мен {1} таңба аралығында болуы керек")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "{0} құрамында кіші әріп, бас әріп және цифр болуы керек")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Құпиясөзді растау")]
    [Required(ErrorMessage = "{0} міндетті түрде толтырылуы керек")]
    [Compare("Password", ErrorMessage = "Құпиясөз бен растау құпиясөзі сәйкес келмейді")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "Пайдалану шарттарымен келісемін")]
    public bool AcceptTerms { get; set; }
}
