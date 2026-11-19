namespace ViewModelValidation.Models;

/// <summary>
/// Domain Model – деректер қорындағы тауардың толық сипаттамасы.
/// Мұнда валидация атрибуттары жоқ: валидация ViewModel деңгейінде жасалады.
/// Id, CreatedAt, IsActive сияқты өрістерді пайдаланушы формадан өзгерте алмауы тиіс.
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int Stock { get; set; }
    public string Category { get; set; } = string.Empty;
    public string SupplierEmail { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
