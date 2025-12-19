using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessProManager.Server.Data;
using FitnessProManager.Server.Models;

namespace FitnessProManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WorkoutsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/workouts?userId=5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkouts(int userId)
        {
            // Authorization: Only return data belonging to the specific User ID
            return await _context.Workouts
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.DateCreated)
                .ToListAsync();
        }

        // POST: api/workouts
        [HttpPost]
        public async Task<ActionResult<Workout>> PostWorkout(Workout workout)
        {
            workout.DateCreated = DateTime.Now;
            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();
            return Ok(workout);
        }

        // DELETE: api/workouts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null) return NotFound();

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}