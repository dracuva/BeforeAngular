using FitnessProManager.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FitnessProManager.Server.Data
{
    // This is the main class that coordinates Entity Framework functions for my models
    // It's the "Librarian" that handles saving and fetching from the actual database
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // These tell the DB to create a "Users" table and a "Workouts" table
        public DbSet<User> Users { get; set; }
        public DbSet<Workout> Workouts { get; set; }

        // This part is for fine-tuning how the tables relate to each other
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicitly setting up the One-to-Many relationship here
            // One User can have many Workouts
            modelBuilder.Entity<User>()
                .HasMany(u => u.Workouts)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId);
        }
    }
}