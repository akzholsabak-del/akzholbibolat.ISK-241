# ViewModelValidation — 4-апта

**Тақырып:** Деректерді жеткізу модельдері (ViewModels) және Валидация логикасы
**Автор:** Ақжол Биболат, ИСК-241
**GitHub:** akzholsabak-del
**Платформа:** ASP.NET Core MVC (.NET 8), Razor, Data Annotations

## Жобаның мақсаты

Domain Model мен ViewModel арасындағы айырмашылықты көрсету, Data Annotations
атрибуттары арқылы деректерді тексеру (Validation) және `ModelState` басқаруды үйрену.

## Жоба құрылымы

| Қалта / файл | Мазмұны |
|---|---|
| `Models/` | Domain Model-дер: `Product`, `AppUser` (валидация атрибуттарынсыз) |
| `ViewModels/` | `ProductCreateViewModel`, `UserRegisterViewModel`, `ProductListItemViewModel`, `RegisterSuccessViewModel` |
| `Validation/` | Өздік атрибуттар: `NoForbiddenWordsAttribute`, `MinimumAgeAttribute` |
| `Controllers/` | `HomeController`, `ProductController`, `AccountController` |
| `Services/` | Жадтағы репозиторийлер және құпиясөзді хэштеу (PBKDF2) |
| `Views/` | Razor көріністері, `_Layout`, `_ValidationScriptsPartial` |
| `wwwroot/js/custom-validation.js` | `MinimumAge` атрибутының клиенттік адаптері |

## Не орындалды

1. **Domain Model және ViewModel бөлінді.** `Product` мен `AppUser`-де `Id`, `CreatedAt`,
   `IsActive`, `PasswordHash` сияқты ішкі өрістер бар; пайдаланушы формасы тек
   ViewModel арқылы келеді (overposting-тен қорғау). `Password` / `ConfirmPassword` тек ViewModel-де.
2. **Data Annotations:** `[Required]`, `[StringLength]`, `[Range]`, `[EmailAddress]`,
   `[Compare("Password")]`, `[RegularExpression]`, `[Display]`, `[DataType]`.
3. **Серверлік валидация:** `HttpPost` әдістерінде `ModelState.IsValid` тексеріледі;
   қате болса, ViewModel сол көріністе қайта қайтарылады.
4. **Қолмен қате қосу (`ModelState.AddModelError`):** артикулдың/логиннің/e-mail-дің бірегейлігі,
   шарттармен келісу, құпиясөзде логин болмауы (модель деңгейіндегі қате `string.Empty` кілтімен).
5. **Razor-да қатені шығару:** `<span asp-validation-for="...">` және
   `<div asp-validation-summary="All">`.
6. **Client-side валидация:** `jquery.validate` + `jquery.validate.unobtrusive`
   (`_ValidationScriptsPartial`), серверлік тексеру әрқашан қосымша қорғаныс ретінде қалады.
7. **Өздік жұмыс:**
   - `IValidatableObject` — `ProductCreateViewModel.Validate()` (жеңілдік бағасы < негізгі баға,
     санат тізімде бар ма, қоймада тауар жоқ кезде жеңілдік);
   - Custom Validation Attribute — `NoForbiddenWords` (тек сервер) және
     `MinimumAge` (`IClientModelValidator` + JS адаптер арқылы клиент пен сервер).

## Іске қосу

1. `ViewModelValidation.sln` файлын Visual Studio Community-де ашыңыз.
2. `F5` немесе `Ctrl+F5` басыңыз (`http://localhost:5150`).
3. Не командалық жолдан: `dotnet run`.

> Жоба `net8.0` үшін жасалған. Visual Studio-да басқа нұсқа болса,
> `ViewModelValidation.csproj` ішіндегі `TargetFramework` мәнін өзгертіңіз (мысалы, `net9.0`).
> jQuery және Bootstrap CDN арқылы жүктеледі, сондықтан интернет қажет.

## Тексеру сценарийлері

| Бет | Енгізу | Күтілетін нәтиже |
|---|---|---|
| Жаңа тауар | Барлығы бос | `[Required]` қателері |
| Жаңа тауар | Атауы «ab» | `[StringLength]` қатесі |
| Жаңа тауар | Артикул «abc-12» | `[RegularExpression]` қатесі |
| Жаңа тауар | Баға 0 | `[Range]` қатесі |
| Жаңа тауар | Жеңілдік ≥ баға | `IValidatableObject` қатесі |
| Жаңа тауар | Артикул `LAP-1001` | `ModelState.AddModelError` (бұрын бар) |
| Жаңа тауар | Атауында «spam» | `NoForbiddenWords` қатесі |
| Тіркелу | Растау құпиясөзі өзгеше | `[Compare]` қатесі |
| Тіркелу | Туған күні 10 жыл бұрын | `MinimumAge(16)` қатесі (клиентте де) |
| Тіркелу | Телефон «87011234567» | `[RegularExpression]` қатесі |
| Тіркелу | Шарттармен келіспеу | `ModelState.AddModelError` |
| Тіркелу | Логин `admin`, құпиясөз `Admin12345` | Модель деңгейіндегі қате (Summary-де) |
