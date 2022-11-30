//using Microsoft.Graph;
//using Azure.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace TechSaturdays.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions _options;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailOptions> options, ILogger<EmailSender> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
