using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Services
{
    public class MultilingualService : IMultilingualService
    {
        private readonly ApplicationDbContext _context;
        private static readonly Dictionary<string, string> _languageMap = new()
        {
            { "en", "English" },
            { "xh", "Xhosa" },
            { "zu", "Zulu" },
            { "af", "Afrikaans" },
            { "st", "Sotho" },
            { "tn", "Tswana" },
            { "ss", "Swati" },
            { "ve", "Venda" },
            { "nr", "Ndebele" },
            { "fr", "Français" }
        };

        public MultilingualService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetTextAsync(string key, string language = "en")
        {
            var content = await _context.MultilingualContents
                .FirstOrDefaultAsync(c => c.Key == key && c.Language == language);

            if (content != null)
                return content.Value;

            // Fallback to English
            if (language != "en")
            {
                content = await _context.MultilingualContents
                    .FirstOrDefaultAsync(c => c.Key == key && c.Language == "en");
                return content?.Value ?? key;
            }

            return key;
        }

        public async Task<Dictionary<string, string>> GetAllTextsAsync(string language = "en")
        {
            var texts = await _context.MultilingualContents
                .Where(c => c.Language == language)
                .ToDictionaryAsync(c => c.Key, c => c.Value);

            return texts;
        }

        public async Task SetUserLanguageAsync(string userId, string language)
        {
            var preference = await _context.LanguagePreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (preference == null)
            {
                preference = new LanguagePreference
                {
                    UserId = userId,
                    PreferredLanguage = language
                };
                _context.LanguagePreferences.Add(preference);
            }
            else
            {
                preference.PreferredLanguage = language;
                preference.SetDate = DateTime.UtcNow;
                _context.LanguagePreferences.Update(preference);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> GetUserLanguageAsync(string userId)
        {
            var preference = await _context.LanguagePreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            return preference?.PreferredLanguage ?? "en";
        }

        public async Task<List<string>> GetAvailableLanguagesAsync()
        {
            return await Task.FromResult(_languageMap.Keys.ToList());
        }
    }
}
