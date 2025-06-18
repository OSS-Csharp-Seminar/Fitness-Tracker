using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkoutPlanRepository : IBaseRepository<WorkoutPlan>
    {
        Task<IQueryable<WorkoutPlan>> GetByUserAsync(User user);
        Task<bool> HasActiveWorkoutPlanAsync(User user);
    }
}