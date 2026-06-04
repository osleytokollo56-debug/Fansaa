using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Services
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Textbook>> SearchTextbooksAsync(SearchFiltersViewModel filters)
        {
            var query = _context.Textbooks
                .Include(t => t.Seller)
                .Where(t => t.IsAvailable);

            // Apply search term
            if (!string.IsNullOrEmpty(filters.SearchTerm))
            {
                var searchLower = filters.SearchTerm.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(searchLower) ||
                    t.Author.ToLower().Contains(searchLower) ||
                    t.ISBN.Contains(filters.SearchTerm) ||
                    t.Description.ToLower().Contains(searchLower));
            }

            // Apply price filter
            if (filters.MinPrice.HasValue)
            {
                query = query.Where(t => t.AskingPrice >= filters.MinPrice.Value);
            }

            if (filters.MaxPrice.HasValue)
            {
                query = query.Where(t => t.AskingPrice <= filters.MaxPrice.Value);
            }

            // Apply condition filter
            if (!string.IsNullOrEmpty(filters.Condition))
            {
                query = query.Where(t => t.Condition == filters.Condition);
            }

            // Apply sorting
            query = filters.SortBy switch
            {
                "price-low" => query.OrderBy(t => t.AskingPrice),
                "price-high" => query.OrderByDescending(t => t.AskingPrice),
                "rating" => query.OrderByDescending(t => t.Seller.AverageRating),
                "newest" => query.OrderByDescending(t => t.ListedDate),
                _ => query.OrderByDescending(t => t.ListedDate)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return new PaginatedResult<Textbook>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filters.Page,
                PageSize = filters.PageSize
            };
        }

        public async Task<PaginatedResult<WantedAd>> SearchWantedAdsAsync(SearchFiltersViewModel filters)
        {
            var query = _context.WantedAds
                .Include(w => w.User)
                .Where(w => w.IsActive);

            // Apply search term
            if (!string.IsNullOrEmpty(filters.SearchTerm))
            {
                var searchLower = filters.SearchTerm.ToLower();
                query = query.Where(w =>
                    w.Title.ToLower().Contains(searchLower) ||
                    w.Author.ToLower().Contains(searchLower) ||
                    w.ISBN.Contains(filters.SearchTerm));
            }

            // Apply price filter
            if (filters.MinPrice.HasValue)
            {
                query = query.Where(w => w.MaxPrice >= filters.MinPrice.Value);
            }

            if (filters.MaxPrice.HasValue)
            {
                query = query.Where(w => w.MaxPrice <= filters.MaxPrice.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(w => w.CreatedDate)
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return new PaginatedResult<WantedAd>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filters.Page,
                PageSize = filters.PageSize
            };
        }

        public async Task<List<string>> GetSearchSuggestionsAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm) || searchTerm.Length < 2)
                return new List<string>();

            var searchLower = searchTerm.ToLower();

            var titles = await _context.Textbooks
                .Where(t => t.IsAvailable && t.Title.ToLower().Contains(searchLower))
                .Select(t => t.Title)
                .Distinct()
                .Take(5)
                .ToListAsync();

            var authors = await _context.Textbooks
                .Where(t => t.IsAvailable && t.Author.ToLower().Contains(searchLower))
                .Select(t => t.Author)
                .Distinct()
                .Take(5)
                .ToListAsync();

            return titles.Concat(authors).Take(10).ToList();
        }
    }
}
