using SomaShare.Models;

namespace SomaShare.Services
{
    public interface ICommunityTrustService
    {
        Task<int> CalculateTrustScoreAsync(string userId);
        Task<string> GetTrustLevelAsync(int score);
        Task<CommunityTrustScoreViewModel> GetUserTrustInfoAsync(string userId);
        Task UpdateTrustScoreAsync(string userId);
    }
}
