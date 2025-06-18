
namespace Domain.Entities
{
    public class Workout 
    {
        public Guid Id { get; set; }
        public Guid? PlanId { get; set; } 
        public Guid CatalogId { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Completed { get; set; }
        public DateOnly Date {  get; set; }

        public virtual WorkoutPlan Plan { get; set; }
        public virtual WorkoutCatalog WorkoutCatalog { get; set; }
    }
}
