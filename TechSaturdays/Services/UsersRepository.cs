using TechSaturdays.Data;
using TechSaturdays.Models;

namespace TechSaturdays.Services
{
    public class UsersRepository : BasicRepository<Guid, ApplicationUser>
    {
        public UsersRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
