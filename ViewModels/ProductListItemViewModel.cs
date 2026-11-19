namespace ViewModelValidation.ViewModels;

/// <summary>Тауарлар тізімін көрсетуге арналған ViewModel.</summary>
public class ProductListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int Stock { get; set; }
}
