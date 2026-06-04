using SomaShare.Models;

namespace SomaShare.Services
{
    public class PasswordSecurityService : IPasswordSecurityService
    {
        private readonly ApplicationDbContext _context;

        public PasswordSecurityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool ValidatePasswordPolicy(string password, PasswordPolicyViewModel policy)
        {
            if (password.Length < policy.MinimumLength)
                return false;

            if (policy.RequireUppercase && !password.Any(char.IsUpper))
                return false;

            if (policy.RequireLowercase && !password.Any(char.IsLower))
                return false;

            if (policy.RequireDigits && !password.Any(char.IsDigit))
                return false;

            if (policy.RequireSpecialCharacters && !password.Any(c => "@$!%*?&".Contains(c)))
                return false;

            return true;
        }

        public string GetPasswordValidationErrors(string password, PasswordPolicyViewModel policy)
        {
            var errors = new List<string>();

            if (password.Length < policy.MinimumLength)
                errors.Add($"Password must be at least {policy.MinimumLength} characters");

            if (policy.RequireUppercase && !password.Any(char.IsUpper))
                errors.Add("Password must contain at least one uppercase letter");

            if (policy.RequireLowercase && !password.Any(char.IsLower))
                errors.Add("Password must contain at least one lowercase letter");

            if (policy.RequireDigits && !password.Any(char.IsDigit))
                errors.Add("Password must contain at least one digit");

            if (policy.RequireSpecialCharacters && !password.Any(c => "@$!%*?&".Contains(c)))
                errors.Add("Password must contain at least one special character (@$!%*?&)");

            return string.Join(", ", errors);
        }

        public async Task<bool> CheckPasswordHistoryAsync(string userId, string newPassword)
        {
            // This is a placeholder - implement password history checking
            return await Task.FromResult(true);
        }

        public async Task StorePasswordHashAsync(string userId, string passwordHash)
        {
            // Store password hash in history table
            await Task.CompletedTask;
        }
    }
}
