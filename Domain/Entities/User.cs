using Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public Gender Gender { get; set; }
        public DateOnly Birthday { get; set; }
        public Role Role { get; set; }
        public float Height { get; set; }

        // Inicijaliziraj liste u konstruktoru ili koristi required
        public List<string> Allergies { get; set; } = new List<string>();
        public List<string> Diagnosis { get; set; } = new List<string>();

        public virtual ICollection<UserPhysique> PhysiqueHistory { get; set; } = new List<UserPhysique>();
        public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
        public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    }
}