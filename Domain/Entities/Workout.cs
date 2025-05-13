using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entities
{
    public class Workout 
    {
        public Guid Id { get; set; }
        public Guid PlanId { get; set; } 
        public string WorkoutName { get; set; } 
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public int CaloriesBurned { get; set; }
        public bool Completed { get; set; }  
        public WorkoutType tag { get; set; }

        public virtual WorkoutPlan Plan { get; set; }
    }
}
