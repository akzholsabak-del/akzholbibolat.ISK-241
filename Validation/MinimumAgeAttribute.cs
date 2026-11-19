using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ViewModelValidation.Validation;

/// <summary>
/// Өздік валидация атрибуты: туған күні бойынша жас кемінде N болуы керек.
/// IClientModelValidator арқылы клиент жағында да (Unobtrusive JavaScript) тексеріледі:
/// HTML-ге data-val-minimumage және data-val-minimumage-years атрибуттары жазылады,
/// ал wwwroot/js/custom-validation.js ішінде сәйкес адаптер тіркелген.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class MinimumAgeAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly int _years;

    public MinimumAgeAttribute(int years)
        : base("{0}: жасыңыз кемінде {1} болуы керек")
    {
        _years = years;
    }

    public override string FormatErrorMessage(string name)
        => string.Format(ErrorMessageString, name, _years);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success; // міндеттілікті [Required] тексереді
        }

        if (value is not DateTime birthDate)
        {
            return new ValidationResult("Күн пішімі қате");
        }

        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }

        if (age < _years)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-minimumage",
            FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        MergeAttribute(context.Attributes, "data-val-minimumage-years", _years.ToString());
    }

    private static void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
    {
        if (!attributes.ContainsKey(key))
        {
            attributes.Add(key, value);
        }
    }
}
