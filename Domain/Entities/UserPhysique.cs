
namespace Domain.Entities
{
    public class UserPhysique 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 
        public float Weight { get; set; }
        public int BMI { get; set; }
        public DateOnly Date {  get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public virtual User User { get; set; }

    }
}
