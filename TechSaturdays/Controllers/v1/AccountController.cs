using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechSaturdays.Interfaces;
using TechSaturdays.Models.InputModels;
using TechSaturdays.Models.ViewModels;

namespace TechSaturdays.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IApplicationAuthenticationService _auth;
        private ILogger<AccountController> _logger;

        public AccountController(IApplicationAuthenticationService auth, ILogger<AccountController> logger)
        {
            _auth = auth;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserAsync()
        {
            var userId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var user = await _auth.GetUserAsync(userId);
            if (user is not null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> PasswordLoginAsync(LoginIM credentials)
        {
            var authResult = await _auth.AuthenticatePasswordAsync(credentials);

            if (authResult.Result.Succeeded)
            {
                return Ok(authResult.AccessToken);
            }
            if (authResult.Result.IsNotAllowed)
            {
                return Unauthorized("Not Allowed");
            }
            if (authResult.Result.IsLockedOut)
            {
                return Unauthorized("Locked Out");
            }
            if (authResult.Result.RequiresTwoFactor)
            {
                return Unauthorized("Two-Factor Required");
            }
            return Unauthorized("Login Failed");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("send-password-recovery")]
        public async Task<IActionResult> SendRecoveryEmailAsync(string email)
        {
            if (await _auth.SendRecoveryEmail(email))
            {
                return Ok();
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("send-email-confirmation")]
        public async Task<IActionResult> SendConfirmationEmailAsync(string email)
        {
            if (await _auth.SendConfirmationEmail(email))
            {
                return Ok();
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("email-confirmation")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
        {
            if (await _auth.ConfirmEmail(userId, code))
            {
                return Ok();
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterIM values)
        {
            var user = await _auth.CreateUserAsync(values);
            if (user == null)
            {
                return BadRequest();
            }
            return Created(nameof(GetUserAsync), user);
        }
    }
}
