using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FitnessProManager.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // This matches the new column in your database
        public string Password { get; set; }

        // This links the user to their specific workouts
        [JsonIgnore]
        public List<Workout> Workouts { get; set; }
    }
}