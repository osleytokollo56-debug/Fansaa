using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Data;
using SomaShare.Services;

namespace SomaShare.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly ApplicationDbContext _context;

        public SearchController(ISearchService searchService, ApplicationDbContext context)
        {
            _searchService = searchService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(SearchFiltersViewModel filters)
        {
            var result = await _searchService.SearchTextbooksAsync(filters);
            ViewBag.Conditions = new[] { "New", "Like New", "Good", "Fair", "Poor" };
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> WantedAds(SearchFiltersViewModel filters)
        {
            var result = await _searchService.SearchWantedAdsAsync(filters);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Suggestions(string term)
        {
            var suggestions = await _searchService.GetSearchSuggestionsAsync(term);
            return Json(suggestions);
        }
    }
}
