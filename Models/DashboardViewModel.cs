namespace SomaShare.Models
{
    public class DashboardViewModel
    {
        // User Stats
        public int TotalListings { get; set; }
        public int ActiveListings { get; set; }
        public int SoldListings { get; set; }
        public decimal TotalEarnings { get; set; }

        // Purchase Stats
        public int TotalPurchases { get; set; }
        public decimal TotalSpent { get; set; }

        // Offer Stats
        public int PendingOffers { get; set; }
        public int AcceptedOffers { get; set; }

        // Rating Info
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int CommunityTrustScore { get; set; }

        // Recent Activity
        public List<RecentActivityViewModel> RecentActivities { get; set; } = new List<RecentActivityViewModel>();

        // Charts Data
        public List<string> MonthLabels { get; set; } = new List<string>();
        public List<int> SalesData { get; set; } = new List<int>();
        public List<decimal> RevenueData { get; set; } = new List<decimal>();
    }

    public class RecentActivityViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; } // "listing", "offer", "transaction", "review"
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Icon { get; set; }
    }
}
