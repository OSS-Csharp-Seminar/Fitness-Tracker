using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entities
{
    public class UserPhysique 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 
        public float Weight { get; set; }
        public float Height { get; set; }
        public DietType dietType { get; set; }
        public List<string> Allergies { get; set; }
        public List<string> Diagnosis { get; set; }
        public DateOnly Date {  get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public virtual User User { get; set; }
    }
}
