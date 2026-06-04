using SomaShare.Models;

namespace SomaShare.Services
{
    public interface IPasswordSecurityService
    {
        bool ValidatePasswordPolicy(string password, PasswordPolicyViewModel policy);
        string GetPasswordValidationErrors(string password, PasswordPolicyViewModel policy);
        Task<bool> CheckPasswordHistoryAsync(string userId, string newPassword);
        Task StorePasswordHashAsync(string userId, string passwordHash);
    }
}
