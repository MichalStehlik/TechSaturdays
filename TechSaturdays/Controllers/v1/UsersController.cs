using Microsoft.AspNetCore.Mvc;
using TechSaturdays.Models;
using TechSaturdays.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechSaturdays.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersRepository _repo;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UsersRepository repo, ILogger<UsersController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<ApplicationUser> Get()
        {
            return _repo.List<string>();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ApplicationUser Get(Guid id)
        {
            return _repo.Read(id);
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] ApplicationUser value)
        {
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ApplicationUser value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var user = _repo.Read(id);
            if (user == null)
            {
                return NotFound();
            }  
            _repo.Delete(user);
            await _repo.SaveAsync();
            return NoContent();
        }
    }
}
