using TechSaturdays.Data;
using TechSaturdays.Models;

namespace TechSaturdays.Services
{
    public class ActionsRepository : BasicRepository<int, EventAction>
    {
        public ActionsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
