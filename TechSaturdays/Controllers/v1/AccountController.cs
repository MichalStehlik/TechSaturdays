using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSaturdays.Interfaces;
using TechSaturdays.Models.InputModels;

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
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterIM values)
        {
            var user = await _auth.CreateUserAsync(values);
            if (user == null)
            {
                return BadRequest();
            }
            return Created(nameof(RegisterAsync), user);
        }
    }
}
