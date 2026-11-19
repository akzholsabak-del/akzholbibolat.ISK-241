using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModelValidation.Models;
using ViewModelValidation.Services;
using ViewModelValidation.ViewModels;

namespace ViewModelValidation.Controllers;

public class ProductController : Controller
{
    private readonly ProductRepository _repository;

    public ProductController(ProductRepository repository)
    {
        _repository = repository;
    }

    // GET: /Product
    public IActionResult Index()
    {
        // Domain Model -> ViewModel
        var items = _repository.GetAll()
            .Select(p => new ProductListItemViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Sku = p.Sku,
                Category = p.Category,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                Stock = p.Stock
            })
            .ToList();

        return View(items);
    }

    // GET: /Product/Create
    [HttpGet]
    public IActionResult Create()
    {
        var model = new ProductCreateViewModel
        {
            Categories = GetCategories()
        };
        return View(model);
    }

    // POST: /Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProductCreateViewModel model)
    {
        // Қолмен қате қосу: бизнес-ереже (артикул бірегей болуы керек)
        if (!string.IsNullOrWhiteSpace(model.Sku) && _repository.SkuExists(model.Sku))
        {
            ModelState.AddModelError(nameof(model.Sku), "Бұл артикулмен тауар бұрын тіркелген");
        }

        if (!ModelState.IsValid)
        {
            // Categories формадан келмейді, сондықтан қайта толтырамыз
            model.Categories = GetCategories();
            return View(model);
        }

        // ViewModel -> Domain Model (тек керекті өрістер көшіріледі)
        var product = new Product
        {
            Name = model.Name.Trim(),
            Sku = model.Sku,
            Category = model.Category,
            Price = model.Price!.Value,
            DiscountPrice = model.DiscountPrice,
            Stock = model.Stock!.Value,
            SupplierEmail = model.SupplierEmail,
            Description = model.Description,
            CreatedAt = DateTime.Now,
            IsActive = true
        };

        _repository.Add(product);

        TempData["Success"] = $"«{product.Name}» тауары сәтті қосылды";
        return RedirectToAction(nameof(Index));
    }

    private static IEnumerable<SelectListItem> GetCategories()
        => ProductCategories.All.Select(c => new SelectListItem(c, c));
}
