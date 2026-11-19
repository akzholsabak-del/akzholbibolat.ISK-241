using System.Globalization;
using Microsoft.AspNetCore.Localization;
using ViewModelValidation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Деректер қоры орнына жадтағы (in-memory) репозиторийлер
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddSingleton<UserRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Index");
}

// Ондық бөлшек үшін нүкте (1234.50) қолданылуы үшін инвариантты мәдениет:
// клиент (jQuery Validation) және сервер бірдей форматты түсінеді.
var culture = new CultureInfo("en-US");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture),
    SupportedCultures = new List<CultureInfo> { culture },
    SupportedUICultures = new List<CultureInfo> { culture }
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
