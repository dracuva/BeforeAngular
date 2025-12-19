using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessProManager.Server.Data;
using FitnessProManager.Server.Models;

namespace FitnessProManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] User loginRequest)
        {
            // Check if a user matches BOTH the name and password
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == loginRequest.Name && u.Password == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(user);
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] User newUser)
        {
            // Check if username is already taken
            if (await _context.Users.AnyAsync(u => u.Name == newUser.Name))
            {
                return BadRequest("Username already taken.");
            }

            // Save new user with password to the database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }
    }
}