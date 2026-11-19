using System.ComponentModel.DataAnnotations;

namespace ViewModelValidation.Validation;

/// <summary>
/// Өздік (Custom) валидация атрибуты: мәтінде тыйым салынған сөздер болмауы керек.
/// ValidationAttribute-тан мұраланып, IsValid әдісі override жасалады.
/// Бұл атрибут тек сервер жағында жұмыс істейді.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class NoForbiddenWordsAttribute : ValidationAttribute
{
    private static readonly string[] Forbidden = { "spam", "fake", "жалған", "алаяқ" };

    public NoForbiddenWordsAttribute()
        : base("{0} өрісінде тыйым салынған сөз бар")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string text && !string.IsNullOrWhiteSpace(text))
        {
            var found = Forbidden.FirstOrDefault(w =>
                text.Contains(w, StringComparison.OrdinalIgnoreCase));

            if (found is not null)
            {
                var members = validationContext.MemberName is null
                    ? null
                    : new[] { validationContext.MemberName };

                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName), members);
            }
        }

        return ValidationResult.Success;
    }
}
