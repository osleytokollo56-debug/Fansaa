using SomaShare.Models;

namespace SomaShare.Services
{
    public interface ISearchService
    {
        Task<PaginatedResult<Textbook>> SearchTextbooksAsync(SearchFiltersViewModel filters);
        Task<PaginatedResult<WantedAd>> SearchWantedAdsAsync(SearchFiltersViewModel filters);
        Task<List<string>> GetSearchSuggestionsAsync(string searchTerm);
    }
}
