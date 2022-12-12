using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System.Security.Claims;
using System.Text.Encodings.Web;
using TechSaturdays.Emails.ViewModels;
using TechSaturdays.Interfaces;
using TechSaturdays.Models.InputModels;
using TechSaturdays.Models.ViewModels;
using TechSaturdays.Services;

namespace TechSaturdays.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IApplicationAuthenticationService _auth;
        private ILogger<AccountController> _logger;
        private readonly IEmailSender _mailer;
        private readonly RazorViewToStringRenderer _renderer;

        public AccountController(
            IApplicationAuthenticationService auth, 
            ILogger<AccountController> logger,
            IEmailSender mailer,
            RazorViewToStringRenderer renderer
            )
        {
            _auth = auth;
            _logger = logger;
            _mailer = mailer;
            _renderer = renderer;
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
            var result = await _auth.CreateUserAsync(values);
            if (!result.Successful)
            {
                return BadRequest();
            }
            string appUrl = HtmlEncoder.Default.Encode(Request.Scheme + "://" + Request.Host.Value);

            string htmlBody = await _renderer.RenderViewToStringAsync("/Emails/Pages/ConfirmAccount.cshtml",
                new ConfirmEmailVM
                {
                    ConfirmationCode = result.ConfirmationCode,
                    User = result.User,
                    ConfirmEmailUrl = appUrl + "/account/email-confirmation?code=" + result.ConfirmationCode + "&id=" + result.User.Id,
                    AppUrl = appUrl
                });

            await _mailer.SendEmailAsync(result.User.Email, "Potvrzení registrace", htmlBody);
            return Created(nameof(GetUserAsync), result.User);
        }
    }
}
