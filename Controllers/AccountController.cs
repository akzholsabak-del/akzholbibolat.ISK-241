using Microsoft.AspNetCore.Mvc;
using ViewModelValidation.Models;
using ViewModelValidation.Services;
using ViewModelValidation.ViewModels;

namespace ViewModelValidation.Controllers;

public class AccountController : Controller
{
    private readonly UserRepository _users;

    public AccountController(UserRepository users)
    {
        _users = users;
    }

    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register() => View(new UserRegisterViewModel());

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(UserRegisterViewModel model)
    {
        // 1) Белгілі бір өріске байланған қолмен қате
        if (!string.IsNullOrWhiteSpace(model.UserName) && _users.UserNameExists(model.UserName))
        {
            ModelState.AddModelError(nameof(model.UserName), "Бұл логин бос емес, басқасын таңдаңыз");
        }

        if (!string.IsNullOrWhiteSpace(model.Email) && _users.EmailExists(model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "Бұл e-mail бұрын тіркелген");
        }

        // 2) Шарттармен келіспесе – қате (checkbox үшін қарапайым серверлік тексеру)
        if (!model.AcceptTerms)
        {
            ModelState.AddModelError(nameof(model.AcceptTerms), "Жалғастыру үшін шарттармен келісу қажет");
        }

        // 3) Модель деңгейіндегі жалпы қате (string.Empty кілті) – тек Validation Summary-де көрінеді
        if (!string.IsNullOrWhiteSpace(model.Password) &&
            !string.IsNullOrWhiteSpace(model.UserName) &&
            model.Password.Contains(model.UserName, StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(string.Empty, "Құпиясөзде логиніңіз қатыспауы керек");
        }

        if (!ModelState.IsValid)
        {
            // Құпиясөз өрістері қайтадан толтырылмайды (Password input әдетте мәнді сақтамайды)
            return View(model);
        }

        var user = new AppUser
        {
            FullName = model.FullName.Trim(),
            UserName = model.UserName,
            Email = model.Email,
            Phone = model.Phone,
            BirthDate = model.BirthDate!.Value,
            Course = model.Course!.Value,
            Bio = model.Bio,
            PasswordHash = PasswordHelper.Hash(model.Password),
            RegisteredAt = DateTime.Now
        };

        _users.Add(user);

        return RedirectToAction(nameof(Success), new { id = user.Id });
    }

    // GET: /Account/Success/5
    public IActionResult Success(int id)
    {
        var user = _users.GetById(id);
        if (user is null)
        {
            return RedirectToAction(nameof(Register));
        }

        var vm = new RegisterSuccessViewModel
        {
            FullName = user.FullName,
            UserName = user.UserName,
            Email = user.Email,
            Phone = user.Phone,
            BirthDate = user.BirthDate,
            Course = user.Course,
            RegisteredAt = user.RegisteredAt
        };

        return View(vm);
    }
}
