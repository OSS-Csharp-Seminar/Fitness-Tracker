using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITreningRepository
    {
        Task<Trening> AddAsync(Trening trening, WorkoutPlan workoutPlan);
        Task<Trening> GetByIdAsync(Guid id);
        Task<IEnumerable<Trening>> GetAllAsync(Guid WorkoutPlanId);

    }
}
