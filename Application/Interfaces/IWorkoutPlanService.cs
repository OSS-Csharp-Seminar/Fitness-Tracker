using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IWorkoutPlanService
    {
        Task<WorkoutPlan> GetWorkoutPlan(WorkoutPlan workoutPlan);
        Task<WorkoutPlan> GetByUserId(Guid userId);
        Task<bool> UpdateWorkoutPlan(WorkoutPlan workoutPlan);
        Task<IEnumerable<WorkoutPlan>> GetAllUserWorkoutPlans(Guid Id);
        Task<bool> DeleteAsync(Guid id, Guid Id);
    }
}
