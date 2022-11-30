using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechSaturdays.Models
{
    public class Application
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public bool Present { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; } = DateTime.Now;
        [Column(TypeName = "datetime2")]
        public DateTime? Revoked { get; set; }
        public ApplicationUser Creator { get; set; }
        public Guid CreatorId { get; set; }
        public ApplicationUser? Revoker { get; set; }
        public Guid? RevokerId { get; set; }
    }
}
