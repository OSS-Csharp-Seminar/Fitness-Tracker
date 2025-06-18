using Domain.Enum;

namespace Domain.Entities
{
    public class WorkoutPlan
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime TargetDate { get; set; } //vise kao procjena da se gubi  kile  1 tjedno 
        public float TargetWeight { get; set; }
        public int TrainingDays { get; set; }
        public TimeOnly TrainingDuration { get; set; }
        public WorkoutPlanType WorkoutPlanType { get; set; }
        public List<WorkoutType> WorkoutPreference { get; set; } = new List<WorkoutType>();
        public int MaxCalories { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}

