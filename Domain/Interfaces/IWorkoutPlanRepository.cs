using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkoutPlanRepository
    {
        Task<WorkoutPlan> AddAsync(WorkoutPlan workoutPlan);
        Task<WorkoutPlan> GetByIdAsync(Guid id);
        Task<WorkoutPlan> GetByUserIdAsync(Guid userId);
        Task<bool> UpdateAsync(WorkoutPlan workoutPlan);
        Task<IEnumerable<WorkoutPlan>> GetAllAsync(Guid userId);
        Task<bool> DeleteAsync(Guid id);
    }
}
