using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetUserDashboardAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return new DashboardViewModel();

            var dashboard = new DashboardViewModel
            {
                AverageRating = user.AverageRating,
                TotalReviews = user.TotalTransactions,
                CommunityTrustScore = await CalculateCommunityTrustScoreAsync(userId)
            };

            // Get recent activities
            dashboard.RecentActivities = await GetRecentActivitiesAsync(userId);

            // Get monthly sales/revenue data
            var (monthLabels, salesData, revenueData) = await GetMonthlyDataAsync(userId);
            dashboard.MonthLabels = monthLabels;
            dashboard.SalesData = salesData;
            dashboard.RevenueData = revenueData;

            return dashboard;
        }

        public async Task<DashboardViewModel> GetSellerDashboardAsync(string userId)
        {
            var dashboard = await GetUserDashboardAsync(userId);

            dashboard.TotalListings = await _context.Textbooks
                .CountAsync(t => t.SellerId == userId);

            dashboard.ActiveListings = await _context.Textbooks
                .CountAsync(t => t.SellerId == userId && t.IsAvailable);

            dashboard.SoldListings = await _context.Textbooks
                .CountAsync(t => t.SellerId == userId && !t.IsAvailable);

            dashboard.TotalEarnings = await _context.Transactions
                .Where(t => t.SellerId == userId)
                .SumAsync(t => t.FinalPrice);

            dashboard.PendingOffers = await _context.Offers
                .CountAsync(o => o.SellerId == userId && o.Status == "Pending");

            return dashboard;
        }

        public async Task<DashboardViewModel> GetBuyerDashboardAsync(string userId)
        {
            var dashboard = await GetUserDashboardAsync(userId);

            dashboard.TotalPurchases = await _context.Transactions
                .CountAsync(t => t.BuyerId == userId);

            dashboard.TotalSpent = await _context.Transactions
                .Where(t => t.BuyerId == userId)
                .SumAsync(t => t.FinalPrice);

            dashboard.AcceptedOffers = await _context.Offers
                .CountAsync(o => o.BuyerId == userId && o.Status == "Accepted");

            dashboard.PendingOffers = await _context.Offers
                .CountAsync(o => o.BuyerId == userId && o.Status == "Pending");

            return dashboard;
        }

        private async Task<int> CalculateCommunityTrustScoreAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return 0;

            int score = 0;

            // Rating component (0-40)
            score += (int)(Math.Min(user.AverageRating / 5.0, 1.0) * 40);

            // Transaction component (0-30)
            var transactionCount = user.TotalTransactions;
            score += Math.Min(transactionCount * 3, 30);

            // Response time component (0-20)
            var avgResponseTime = await GetAverageResponseTimeAsync(userId);
            if (avgResponseTime < 1) score += 20;
            else if (avgResponseTime < 3) score += 15;
            else if (avgResponseTime < 7) score += 10;

            // Completion rate (0-10)
            var completionRate = await GetCompletionRateAsync(userId);
            score += (int)(completionRate * 10);

            return Math.Min(score, 100);
        }

        private async Task<double> GetAverageResponseTimeAsync(string userId)
        {
            var avgDays = await _context.Offers
                .Where(o => o.SellerId == userId && o.ResponseDate.HasValue)
                .AverageAsync(o => EF.Functions.DateDiffDay(o.CreatedDate, o.ResponseDate.Value));

            return avgDays;
        }

        private async Task<double> GetCompletionRateAsync(string userId)
        {
            var totalOffers = await _context.Offers
                .CountAsync(o => o.SellerId == userId);

            if (totalOffers == 0) return 0;

            var completedOffers = await _context.Offers
                .CountAsync(o => o.SellerId == userId && o.Status == "Accepted");

            return (double)completedOffers / totalOffers;
        }

        private async Task<List<RecentActivityViewModel>> GetRecentActivitiesAsync(string userId)
        {
            var activities = new List<RecentActivityViewModel>();

            // Recent listings
            var listings = await _context.Textbooks
                .Where(t => t.SellerId == userId)
                .OrderByDescending(t => t.ListedDate)
                .Take(3)
                .Select(t => new RecentActivityViewModel
                {
                    Id = t.Id,
                    Type = "listing",
                    Description = $"Listed {t.Title}",
                    Date = t.ListedDate,
                    Icon = "📚"
                })
                .ToListAsync();

            activities.AddRange(listings);

            // Recent transactions
            var transactions = await _context.Transactions
                .Where(t => t.BuyerId == userId || t.SellerId == userId)
                .OrderByDescending(t => t.TransactionDate)
                .Take(3)
                .Select(t => new RecentActivityViewModel
                {
                    Id = t.Id,
                    Type = "transaction",
                    Description = t.BuyerId == userId ? "Purchased book" : "Sold book",
                    Date = t.TransactionDate,
                    Icon = "💰"
                })
                .ToListAsync();

            activities.AddRange(transactions);
            return activities.OrderByDescending(a => a.Date).Take(5).ToList();
        }

        private async Task<(List<string> months, List<int> sales, List<decimal> revenue)> GetMonthlyDataAsync(string userId)
        {
            var months = new List<string>();
            var sales = new List<int>();
            var revenue = new List<decimal>();

            var last12Months = Enumerable.Range(0, 12)
                .Select(i => DateTime.UtcNow.AddMonths(-i))
                .OrderBy(d => d)
                .ToList();

            foreach (var month in last12Months)
            {
                months.Add(month.ToString("MMM yyyy"));

                var monthSales = await _context.Transactions
                    .Where(t => t.SellerId == userId &&
                           t.TransactionDate.Year == month.Year &&
                           t.TransactionDate.Month == month.Month)
                    .CountAsync();
                sales.Add(monthSales);

                var monthRevenue = await _context.Transactions
                    .Where(t => t.SellerId == userId &&
                           t.TransactionDate.Year == month.Year &&
                           t.TransactionDate.Month == month.Month)
                    .SumAsync(t => t.FinalPrice);
                revenue.Add(monthRevenue);
            }

            return (months, sales, revenue);
        }
    }
}
