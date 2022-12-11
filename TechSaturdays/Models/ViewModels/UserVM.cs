using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechSaturdays.Models.ViewModels
{
    public class UserVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public DateTime BirthDate { get; set; }
        public string? School { get; set; } = String.Empty;
        public Grade Grade { get; set; } = Grade.None;
        public bool Aspirant { get; set; } = false;
        public bool InMailingList { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
