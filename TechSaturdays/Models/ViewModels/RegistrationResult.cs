namespace TechSaturdays.Models.ViewModels
{
    public record RegistrationResult
    {
        public bool Successful { get; set; }
        public ApplicationUser User { get; set; }
        public string ConfirmationCode { get; set; } = string.Empty;
    }
}
