using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModelValidation.Models;
using ViewModelValidation.Validation;

namespace ViewModelValidation.ViewModels;

/// <summary>
/// Жаңа тауар қосуға арналған ViewModel.
/// - Product domain моделінде жоқ қосымша деректерді (Categories тізімі) көрінісіне береді;
/// - Id, CreatedAt, IsActive өрістерін қамтымайды (overposting-тен қорғау);
/// - Data Annotations арқылы валидация; күрделі ережелер IValidatableObject арқылы.
/// </summary>
public class ProductCreateViewModel : IValidatableObject
{
    [Display(Name = "Тауар атауы")]
    [Required(ErrorMessage = "{0} міндетті түрде толтырылуы керек")]
    [StringLength(100, MinimumLength = 3,
        ErrorMessage = "{0} ұзындығы {2} мен {1} таңба аралығында болуы керек")]
    [NoForbiddenWords]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Артикул (SKU)")]
    [Required(ErrorMessage = "{0} міндетті түрде толтырылуы керек")]
    [RegularExpression(@"^[A-Z]{3}-\d{4}$",
        ErrorMessage = "{0} пішімі: 3 бас латын әріп, сызықша, 4 цифр (мысалы, LAP-1234)")]
    public string Sku { get; set; } = string.Empty;

    [Display(Name = "Санат")]
    [Required(ErrorMessage = "{0} таңдаңыз")]
    public string Category { get; set; } = string.Empty;

    [Display(Name = "Бағасы (₸)")]
    [Required(ErrorMessage = "{0} көрсетілуі керек")]
    [Range(1, 10000000, ErrorMessage = "{0} {1} мен {2} аралығында болуы керек")]
    public decimal? Price { get; set; }

    [Display(Name = "Жеңілдік бағасы (₸)")]
    [Range(1, 10000000, ErrorMessage = "{0} {1} мен {2} аралығында болуы керек")]
    public decimal? DiscountPrice { get; set; }

    [Display(Name = "Қоймадағы саны")]
    [Required(ErrorMessage = "{0} көрсетілуі керек")]
    [Range(0, 10000, ErrorMessage = "{0} {1} мен {2} аралығында болуы керек")]
    public int? Stock { get; set; }

    [Display(Name = "Жеткізуші e-mail")]
    [Required(ErrorMessage = "{0} міндетті түрде толтырылуы керек")]
    [EmailAddress(ErrorMessage = "{0} пішімі дұрыс емес")]
    public string SupplierEmail { get; set; } = string.Empty;

    [Display(Name = "Сипаттамасы")]
    [StringLength(500, ErrorMessage = "{0} {1} таңбадан аспауы керек")]
    [NoForbiddenWords]
    public string? Description { get; set; }

    // Көрініске арналған қосымша деректер: формадан байланыспайды және валидацияланбайды
    [BindNever]
    [ValidateNever]
    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

    /// <summary>
    /// Бірнеше өрісті салыстыратын күрделі ереже.
    /// Бұл әдіс атрибуттар қатесіз өткеннен кейін ғана шақырылады.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Price.HasValue && DiscountPrice.HasValue && DiscountPrice.Value >= Price.Value)
        {
            yield return new ValidationResult(
                "Жеңілдік бағасы негізгі бағадан кем болуы керек",
                new[] { nameof(DiscountPrice) });
        }

        if (!string.IsNullOrWhiteSpace(Category) &&
            !ProductCategories.All.Contains(Category))
        {
            yield return new ValidationResult(
                "Таңдалған санат тізімде жоқ",
                new[] { nameof(Category) });
        }

        if (Stock == 0 && DiscountPrice.HasValue)
        {
            yield return new ValidationResult(
                "Қоймада тауар жоқ кезде жеңілдік бағасын көрсетудің мәні жоқ");
        }
    }
}
