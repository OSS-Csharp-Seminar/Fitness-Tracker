using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TreningWorkout
    {
        public Guid Id { get; set; }
        public Guid TreningId { get; set; }
        public Guid WorkoutId { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public bool Completed { get; set; }

        public virtual Trening Trening { get; set; }
        public virtual Workout Workout { get; set; }
    }
}
