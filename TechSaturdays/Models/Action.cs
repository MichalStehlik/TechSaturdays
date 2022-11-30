namespace TechSaturdays.Models
{
    public class Action
    {
        public int ActionId { get; set; }
        public string Name { get; set; } = "X. sobota s technikou";
        public int Year { get; set; } = DateTime.Now.Year;
        public string Description { get; set; } = String.Empty;
        public bool Active { get; set; } = true;
        public bool IsPublished { get; set; } = false;
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public ICollection<Group> Groups { get; set; }
    }
}
