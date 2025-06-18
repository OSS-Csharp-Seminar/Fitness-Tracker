using Domain.Enum;

namespace Domain.Entities
{
    public class WorkoutCatalog
    {
        public Guid Id { get; set; }
        public string WorkoutName { get; set; }
        public string Description { get; set; }
        public int CaloriesBurned { get; set; }
        public List<WorkoutType> tag { get; set; } = new List<WorkoutType>();
        public string? ImageUrl { get; set; }

        public virtual Workout Workout { get; set; }

    }
}
