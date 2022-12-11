using System.ComponentModel.DataAnnotations;

namespace TechSaturdays.Models
{
    public class EventAction
    {
        [Key]
        public int ActionId { get; set; }
        [Required]
        public string Name { get; set; } = "X. sobota s technikou";
        [Required]
        public int Year { get; set; } = DateTime.Now.Year;
        public string Description { get; set; } = String.Empty;
        public bool Active { get; set; } = true;
        public bool IsPublished { get; set; } = false;
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
        [Required]
        public DateTime Updated { get; set; } = DateTime.Now;
        public ICollection<Group> Groups { get; set; }
    }
}
