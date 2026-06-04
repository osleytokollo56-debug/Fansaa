using System.ComponentModel.DataAnnotations;

namespace SomaShare.Models
{
    public class SearchFiltersViewModel
    {
        [Display(Name = "Search Term")]
        public string SearchTerm { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Min Price")]
        [DataType(DataType.Currency)]
        public decimal? MinPrice { get; set; }

        [Display(Name = "Max Price")]
        [DataType(DataType.Currency)]
        public decimal? MaxPrice { get; set; }

        [Display(Name = "Condition")]
        public string Condition { get; set; }

        [Display(Name = "Sort By")]
        public string SortBy { get; set; } = "newest"; // newest, price-low, price-high, rating

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }

    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
