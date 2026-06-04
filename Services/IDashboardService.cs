using SomaShare.Models;

namespace SomaShare.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetUserDashboardAsync(string userId);
        Task<DashboardViewModel> GetSellerDashboardAsync(string userId);
        Task<DashboardViewModel> GetBuyerDashboardAsync(string userId);
    }
}
