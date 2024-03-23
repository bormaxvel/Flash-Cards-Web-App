using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace FlashCards.Controllers
{
    public class SelectLanguageController : Controller
    {
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var cultures = ((requestCulture as RequestCultureFeature)?.Provider as RequestCultureProvider)?.Options?.SupportedUICultures;
            return View(cultures);
        }

        [HttpPost]
        public IActionResult SetLanguage(string cultureName, string returnUrl = null)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureName)), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else 
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
