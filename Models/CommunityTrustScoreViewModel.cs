namespace SomaShare.Models
{
    public class CommunityTrustScoreViewModel
    {
        public int UserId { get; set; }
        public int Score { get; set; } // 0-100
        public int TransactionCount { get; set; }
        public double AverageRating { get; set; }
        public int PositiveReviews { get; set; }
        public int NegativeReviews { get; set; }
        public int Responses { get; set; } // Quick response score
        public int Reliability { get; set; } // On-time delivery/completion
        public string TrustLevel { get; set; } // Bronze, Silver, Gold, Platinum
        public DateTime LastUpdated { get; set; }
        public string Badge { get; set; } // Icon/badge URL
    }
}
