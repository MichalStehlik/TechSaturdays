using Microsoft.AspNetCore.Identity.UI.Services;

namespace TechSaturdays.Interfaces
{
    public interface IHtmlEmailSender : IEmailSender
    {
        Task SendEmailAsync(String email, String subject, String htmlMessage, String plainMessage);
    }
}
