using System.ComponentModel.DataAnnotations;

namespace TechSaturdays.Models.InputModels
{
    public class RegisterIM
    {
        public string Email { get; set; } = String.Empty;
        [Required]
        public string Firstname { get; set; } = String.Empty;
        [Required]
        public string Lastname { get; set; } = String.Empty;
        public DateTime BirthDate { get; set; }
        public string School { get; set; } = String.Empty;
        public Grade Grade { get; set; } = Grade.None;
        public bool Aspirant { get; set; } = false;
        public bool InMailingList { get; set; } = false;
        public string Password { get; set; } = String.Empty;
    }
}
