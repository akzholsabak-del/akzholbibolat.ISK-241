using Microsoft.AspNetCore.Mvc;

namespace ViewModelValidation.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
