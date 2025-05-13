using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Trening
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid WorkoutPlanId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public int CaloriesBurned { get; set; }
        public bool Completed { get; set; }

        public virtual User User { get; set; }
        public virtual WorkoutPlan WorkoutPlan { get; set; }
        public virtual ICollection<TreningWorkout> TreningWorkouts { get; set; } = new List<TreningWorkout>();
    }
}
