namespace SomaShare.Services
{
    public interface IMultilingualService
    {
        Task<string> GetTextAsync(string key, string language = "en");
        Task<Dictionary<string, string>> GetAllTextsAsync(string language = "en");
        Task SetUserLanguageAsync(string userId, string language);
        Task<string> GetUserLanguageAsync(string userId);
        Task<List<string>> GetAvailableLanguagesAsync();
    }
}
