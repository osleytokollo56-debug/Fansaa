using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Services
{
    public class CommunityTrustService : ICommunityTrustService
    {
        private readonly ApplicationDbContext _context;

        public CommunityTrustService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CalculateTrustScoreAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return 0;

            int score = 0;

            // Rating component (0-40)
            score += (int)(Math.Min(user.AverageRating / 5.0, 1.0) * 40);

            // Transaction component (0-30)
            score += Math.Min(user.TotalTransactions * 2, 30);

            // Review sentiment analysis (0-20)
            var positiveReviews = await _context.Reviews
                .CountAsync(r => r.ReviewedUserId == userId && r.Rating >= 4);
            var totalReviews = await _context.Reviews
                .CountAsync(r => r.ReviewedUserId == userId);

            if (totalReviews > 0)
            {
                var positiveRatio = (double)positiveReviews / totalReviews;
                score += (int)(positiveRatio * 20);
            }

            // Account age bonus (0-10)
            var user_obj = await _context.Users.FindAsync(userId);
            var accountAge = DateTime.UtcNow - user_obj.RegistrationDate;
            if (accountAge.Days > 365) score += 10;
            else if (accountAge.Days > 180) score += 5;

            return Math.Min(score, 100);
        }

        public async Task<string> GetTrustLevelAsync(int score)
        {
            return score switch
            {
                >= 80 => "Platinum",
                >= 60 => "Gold",
                >= 40 => "Silver",
                _ => "Bronze"
            };
        }

        public async Task<CommunityTrustScoreViewModel> GetUserTrustInfoAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            var score = await CalculateTrustScoreAsync(userId);
            var trustLevel = await GetTrustLevelAsync(score);

            var reviews = await _context.Reviews
                .Where(r => r.ReviewedUserId == userId)
                .ToListAsync();

            var positiveReviews = reviews.Count(r => r.Rating >= 4);
            var negativeReviews = reviews.Count(r => r.Rating < 4);

            var offers = await _context.Offers
                .Where(o => o.SellerId == userId)
                .ToListAsync();

            var responses = offers.Count(o => o.ResponseDate.HasValue);

            return new CommunityTrustScoreViewModel
            {
                Score = score,
                TransactionCount = user.TotalTransactions,
                AverageRating = user.AverageRating,
                PositiveReviews = positiveReviews,
                NegativeReviews = negativeReviews,
                Responses = responses,
                TrustLevel = trustLevel,
                LastUpdated = DateTime.UtcNow,
                Badge = GetBadgeUrl(trustLevel)
            };
        }

        public async Task UpdateTrustScoreAsync(string userId)
        {
            // This would update cached trust scores in the database if needed
            var score = await CalculateTrustScoreAsync(userId);
            // Store in a TrustScore table if needed for performance
            await Task.CompletedTask;
        }

        private string GetBadgeUrl(string trustLevel)
        {
            return trustLevel switch
            {
                "Platinum" => "/images/badges/platinum.png",
                "Gold" => "/images/badges/gold.png",
                "Silver" => "/images/badges/silver.png",
                _ => "/images/badges/bronze.png"
            };
        }
    }
}
