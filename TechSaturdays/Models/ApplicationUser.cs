using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechSaturdays.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        public string FirstName { get; set; } = String.Empty;
        [Required]
        public string LastName { get; set; } = String.Empty;
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime BirthDate { get; set; }
        public string? School { get; set; } = String.Empty;
        public Grade Grade { get; set; } = Grade.None;
        public bool Aspirant { get; set; } = false;
        public bool InMailingList { get; set; } = false;
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; } = DateTime.Now;
        [Column(TypeName = "datetime2")]
        public DateTime Updated { get; set; } = DateTime.Now;
        [Column(TypeName = "datetime2")]
        public ICollection<Application> Applications { get; set; }
        public ICollection<Application> CreatedApplications { get; set; }
        public ICollection<Application> RevokedApplications { get; set; }
        public ICollection<Group> GroupsLectored { get; set; }
    }
}
