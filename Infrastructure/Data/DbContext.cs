using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        public DbSet<WorkoutCatalog> WorkoutCatalogs { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // Ispravi nullable liste
            var allergiesProperty = modelBuilder.Entity<User>()
                .Property(u => u.Allergies)
                .HasConversion(
                    v => v != null && v.Any() ? string.Join(',', v) : string.Empty,
                    v => string.IsNullOrEmpty(v) ? new List<string>() : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

            allergiesProperty.Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                c => c != null ? c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())) : 0,
                c => c != null ? c.ToList() : new List<string>()));

            var diagnosisProperty = modelBuilder.Entity<User>()
                .Property(u => u.Diagnosis)
                .HasConversion(
                    v => v != null && v.Any() ? string.Join(',', v) : string.Empty,
                    v => string.IsNullOrEmpty(v) ? new List<string>() : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

            diagnosisProperty.Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                c => c != null ? c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())) : 0,
                c => c != null ? c.ToList() : new List<string>()));

            var tagProperty = modelBuilder.Entity<WorkoutCatalog>()
                .Property(w => w.tag)
                .HasConversion(
                    v => v != null && v.Any() ? string.Join(',', v) : string.Empty,
                    v => string.IsNullOrEmpty(v) ? new List<WorkoutType>() : v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(x => Enum.Parse<WorkoutType>(x))
                          .ToList());

            tagProperty.Metadata.SetValueComparer(new ValueComparer<List<WorkoutType>>(
                (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                c => c != null ? c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())) : 0,
                c => c != null ? c.ToList() : new List<WorkoutType>()));

            modelBuilder.Entity<WorkoutCatalog>()
                .Property(w => w.ImageUrl)
                .HasMaxLength(500);

            modelBuilder.Entity<WorkoutPlan>()
                .Property(wp => wp.WorkoutPlanType)
                .HasConversion<string>();

            var workoutPreferenceProperty = modelBuilder.Entity<WorkoutPlan>()
                .Property(wp => wp.WorkoutPreference)
                .HasConversion(
                    v => v != null && v.Any() ? string.Join(',', v) : string.Empty,
                    v => string.IsNullOrEmpty(v) ? new List<WorkoutType>() : v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(x => Enum.Parse<WorkoutType>(x))
                          .ToList());

            workoutPreferenceProperty.Metadata.SetValueComparer(new ValueComparer<List<WorkoutType>>(
                (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                c => c != null ? c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())) : 0,
                c => c != null ? c.ToList() : new List<WorkoutType>()));

            // Relationshipi ostaju isti
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
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<WorkoutCatalog>()
                .HasOne(wc => wc.Workout)
                .WithOne(w => w.WorkoutCatalog)
                .HasForeignKey<Workout>(w => w.CatalogId)
                .OnDelete(DeleteBehavior.Restrict);

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

            // Indexi ostaju isti
            modelBuilder.Entity<UserPhysique>()
                .HasIndex(up => new { up.UserId, up.Date })
                .HasDatabaseName("IX_UserPhysique_User_Date");

            modelBuilder.Entity<Workout>()
                .HasIndex(w => new { w.PlanId, w.Date })
                .HasDatabaseName("IX_Workout_Plan_Date");

            modelBuilder.Entity<Workout>()
                .HasIndex(w => w.CatalogId)
                .HasDatabaseName("IX_Workout_Catalog");

            modelBuilder.Entity<Message>()
                .HasIndex(m => new { m.SenderId, m.TrainerId, m.DateSent })
                .HasDatabaseName("IX_Message_Conversation");
        }
    }
}