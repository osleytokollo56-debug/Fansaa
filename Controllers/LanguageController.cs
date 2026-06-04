using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;
using SomaShare.Services;

namespace SomaShare.Controllers
{
    public class LanguageController : Controller
    {
        private readonly IMultilingualService _multilingualService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LanguageController(IMultilingualService multilingualService, UserManager<ApplicationUser> userManager)
        {
            _multilingualService = multilingualService;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetLanguage(string language)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                await _multilingualService.SetUserLanguageAsync(user.Id, language);
            }

            // Set cookie for non-authenticated users
            Response.Cookies.Append("UserLanguage", language);

            return Redirect(Request.Headers["Referer"].ToString() ?? "/");
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableLanguages()
        {
            var languages = await _multilingualService.GetAvailableLanguagesAsync();
            return Json(languages);
        }
    }
}
