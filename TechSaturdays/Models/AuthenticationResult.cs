using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace TechSaturdays.Models
{
    public struct AuthenticationResult
    {
        public SignInResult Result { get; set; }
        public AuthenticationToken? AccessToken { get; set; }
    }
}
