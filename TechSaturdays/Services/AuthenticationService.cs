using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechSaturdays.Interfaces;
using TechSaturdays.Models;
using TechSaturdays.Models.InputModels;

namespace TechSaturdays.Services
{
    public sealed class AuthenticationService : IApplicationAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly UserManager<ApplicationUser> _um;
        private readonly JWTOptions _options;

        public AuthenticationService(ILogger<AuthenticationService> logger, SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, IOptions<JWTOptions> options)
        {
            _logger = logger;
            _sim = sim;
            _um = um;
            _options = options.Value;
        }

        public async Task<AuthenticationResult> AuthenticatePasswordAsync(LoginIM credentials)
        {
            SignInResult result = await _sim.PasswordSignInAsync(credentials.Username, credentials.Password, true, true);
            if (!result.Succeeded)
            {
                _logger.LogInformation("User " + credentials.Username + " login resulted in " + result.ToString());
                return new AuthenticationResult { Result = result, AccessToken = null };
            }
            _logger.LogInformation("User " + credentials.Username + " logged in.");
            var user = await _um.FindByNameAsync(credentials.Username);
            if (user == null)
            {
                return new AuthenticationResult { Result = result, AccessToken = null };
            }
            AuthenticationToken? token = await GenerateAuthenticationToken(user);
            return new AuthenticationResult { Result = result, AccessToken = token };
        }

        public async Task<ApplicationUser?> CreateUserAsync(RegisterIM entry)
        {
            ApplicationUser user = new ApplicationUser
            {
                Firstname = entry.Firstname,
                Lastname = entry.Lastname,
                Email = entry.Email,
                UserName = entry.Email,
                School = entry.School,
                InMailingList = entry.InMailingList,
                BirthDate = entry.BirthDate,
                Created = DateTime.Now,
                Updated = DateTime.Now,
            };
            var result = await _um.CreateAsync(user, entry.Password);
            if (!result.Succeeded)
            {
                return null;
            }
            return user;
        }

        private async Task<AuthenticationToken?> GenerateAuthenticationToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_options.Key);
            var claims = new List<Claim> 
            {
                new(ClaimTypes.Name, user.UserName),
                new("sub", user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Surname, user.Lastname),
                new(ClaimTypes.GivenName, user.Firstname)
            };
            var userClaims = await _um.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt= DateTime.UtcNow,
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_options.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthenticationToken{ Name = "authentication_token", Value = tokenHandler.WriteToken(token)};
        }
    }
}
