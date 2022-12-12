using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using TechSaturdays.Controllers.v1;
using TechSaturdays.Emails.ViewModels;
using TechSaturdays.Interfaces;
using TechSaturdays.Models;
using TechSaturdays.Models.InputModels;
using TechSaturdays.Models.ViewModels;

namespace TechSaturdays.Services
{
    public sealed class AuthenticationService : IApplicationAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly UserManager<ApplicationUser> _um;
        private readonly JWTOptions _options;
        private readonly IEmailSender _mailer;
        private readonly RazorViewToStringRenderer _renderer;
        private readonly IHttpContextAccessor _hca;
        private readonly HttpRequest _request;

        public AuthenticationService(
            ILogger<AuthenticationService> logger, 
            SignInManager<ApplicationUser> sim, 
            UserManager<ApplicationUser> um, 
            IOptions<JWTOptions> options, 
            IEmailSender mailer, 
            RazorViewToStringRenderer renderer,
            IHttpContextAccessor hca
            )
        {
            _logger = logger;
            _sim = sim;
            _um = um;
            _options = options.Value;
            _mailer = mailer;
            _renderer = renderer;
            _hca = hca;
            _request = hca.HttpContext.Request;
        }

        public async Task<AuthenticationResult> AuthenticatePasswordAsync(LoginIM credentials)
        {
            SignInResult result = await _sim.PasswordSignInAsync(credentials.Username, credentials.Password, true, true);
            if (!result.Succeeded)
            {
                _logger.LogInformation("User " + credentials.Username + " login resulted in " + result.ToString());
                return new AuthenticationResult(result, null);
            }
            _logger.LogInformation("User " + credentials.Username + " logged in.");
            var user = await _um.FindByNameAsync(credentials.Username);
            if (user == null)
            {
                return new AuthenticationResult(result, null);
            }
            AuthenticationToken? token = await GenerateAuthenticationToken(user);
            return new AuthenticationResult(result, token);
        }

        public async Task<RegistrationResult> CreateUserAsync(RegisterIM entry)
        {
            ApplicationUser user = new ApplicationUser
            {
                FirstName = entry.Firstname,
                LastName = entry.Lastname,
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
                return new RegistrationResult { Successful = false };
            }
            var code = await _um.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            return new RegistrationResult { Successful = true, User = user, ConfirmationCode = code};
        }

        public async Task<UserVM?> GetUserAsync(Guid id)
        {
            ApplicationUser user = await _um.FindByIdAsync(id.ToString());
            if (user is not null)
            {
                return new UserVM
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    School = user.School,
                    Aspirant = user.Aspirant,
                    InMailingList = user.InMailingList,
                    BirthDate = user.BirthDate,
                };
            }
            return null;
        }

        public async Task<bool> SendRecoveryEmail(string email)
        {
            var user = await _um.FindByEmailAsync(email);
            if (user == null || !(await _um.IsEmailConfirmedAsync(user)))
            {
                return false;
            }
            var userId = await _um.GetUserIdAsync(user);
            var code = await _um.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await _mailer.SendEmailAsync(
                    email,
                    "Password Recovery",
                    $"CODE:" + code + " UID:" + userId);
            return true;
        }

        public async Task<bool> SendConfirmationEmail(string email)
        {
            var user = await _um.FindByEmailAsync(email);
            if (user == null || !(await _um.IsEmailConfirmedAsync(user)))
            {
                return false;
            }
            var userId = await _um.GetUserIdAsync(user);
            var code = await _um.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await _mailer.SendEmailAsync(
                    email,
                    "Email Confirmation",
                    $"CODE:" + code + " UID:" + userId);
            return true;
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            var user = await _um.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _um.ConfirmEmailAsync(user, code);
            if (result.Succeeded) 
            { 
                return true; 
            }
            return false;
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
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.GivenName, user.FirstName)
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

    public record AuthenticationResult(SignInResult Result, AuthenticationToken? AccessToken);
}
