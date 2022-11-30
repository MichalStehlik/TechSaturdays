namespace TechSaturdays.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = String.Empty;
        public int ActionId { get; set; }
        public Action Action { get; set; }
        public string Description { get; set; } = String.Empty;
        public string Note { get; set; } = String.Empty;
        public int MaximumCapacity { get; set; } = 0;
        public Grade MinimalGrade { get; set; }
        public bool IsVisible { get; set; } = false;
        public bool IsCapacityVisible { get; set; } = false;
        public bool IsUnlocked { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public ICollection<ApplicationUser> Lectors { get; set; }
        public ICollection<Application> Applications { get; set; }

    }
}
