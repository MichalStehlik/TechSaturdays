using Microsoft.AspNetCore.Authentication;
using TechSaturdays.Models;
using TechSaturdays.Models.InputModels;
using TechSaturdays.Models.ViewModels;
using TechSaturdays.Services;

namespace TechSaturdays.Interfaces
{
    public interface IApplicationAuthenticationService
    {
        public Task<UserVM?> GetUserAsync(Guid id);
        public Task<AuthenticationResult> AuthenticatePasswordAsync(LoginIM credentials);
        public Task<RegistrationResult> CreateUserAsync(RegisterIM entry);
        public Task<RegistrationResult> SendRecoveryEmail(string email);
        public Task<RegistrationResult> SendConfirmationEmail(string email);
        public Task<bool> ConfirmEmail(string id, string code);
    }
}
