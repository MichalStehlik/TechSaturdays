using TechSaturdays.Models;

namespace TechSaturdays.Emails.ViewModels
{
    public class ConfirmEmailVM
    {
        public string ConfirmationCode { get; set; } = String.Empty;
        public ApplicationUser User { get; set; }
        public string ConfirmEmailUrl { get; set; } = String.Empty;
        public string AppUrl { get; set; } = String.Empty;
    }
}
