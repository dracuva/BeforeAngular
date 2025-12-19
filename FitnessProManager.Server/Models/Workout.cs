namespace FitnessProManager.Server.Models
{
    // My model for the workout data
    public class Workout
    {
        public int Id { get; set; }
        public string Activity { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        // --- IMPORTANT: This is the One-to-Many link ---
        // This links the workout to a specific User ID
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}