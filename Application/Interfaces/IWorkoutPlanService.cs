using Domain.Entities;

namespace Application.Interfaces
{
    public interface IWorkoutPlanService
    {
        Task<WorkoutPlan> CreateWorkoutPlan(WorkoutPlan workoutPlan);
        Task<WorkoutPlan> GetWorkoutPlanById(Guid id);
        Task<List<WorkoutPlan>> GetByUser(User user);
        Task<bool> DeleteWorkoutPlan(Guid id);

        Task<List<Workout>> GetOrGenerateWorkoutsForToday(User user);
        Task<List<Workout>> GetWorkoutHistory(User user, DateOnly? fromDate = null, DateOnly? toDate = null);
        int CalculateMaxCalories(float currentWeight, float targetWeight, string lifestyleType);
    }

}
