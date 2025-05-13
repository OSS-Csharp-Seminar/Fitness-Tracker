using System;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserPhysique> UserPhysiques { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Message> Messages { get; set; }

        // Novi DbSets za Trening i TreningWorkout
        public DbSet<Trening> Trenings { get; set; }
        public DbSet<TreningWorkout> TreningWorkouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Workout>()
                .Property(u => u.tag)
                .HasConversion<string>();

            modelBuilder.Entity<UserPhysique>()
                .Property(up => up.dietType)
                .HasConversion<string>();

            modelBuilder.Entity<WorkoutPlan>()
                .Property(wp => wp.WorkoutPlanType)
                .HasConversion<string>();

            modelBuilder.Entity<UserPhysique>()
                .HasOne(up => up.User)
                .WithMany(u => u.PhysiqueHistory)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutPlan>()
                .HasOne(wp => wp.User)
                .WithMany(u => u.WorkoutPlans)
                .HasForeignKey(wp => wp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Workout>()
                .HasOne(w => w.Plan)
                .WithMany(wp => wp.Workouts)
                .HasForeignKey(w => w.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Meal>()
                .HasOne(m => m.User)
                .WithMany(u => u.Meals)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Trainer)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trening>()
                .HasOne(t => t.User)
                .WithMany(u => u.Trenings)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trening>()
                .HasOne(t => t.WorkoutPlan)
                .WithMany(wp => wp.Trenings)
                .HasForeignKey(t => t.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TreningWorkout>()
                .HasOne(tw => tw.Trening)
                .WithMany(t => t.TreningWorkouts)
                .HasForeignKey(tw => tw.TreningId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TreningWorkout>()
                .HasOne(tw => tw.Workout)
                .WithMany()
                .HasForeignKey(tw => tw.WorkoutId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}