using Microsoft.AspNetCore.Authentication;
using TechSaturdays.Models;
using TechSaturdays.Models.InputModels;

namespace TechSaturdays.Interfaces
{
    public interface IApplicationAuthenticationService
    {
        public Task<AuthenticationResult> AuthenticatePasswordAsync(LoginIM credentials);
        public Task<ApplicationUser?> CreateUserAsync(RegisterIM entry);
    }
}
