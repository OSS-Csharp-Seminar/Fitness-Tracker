using Domain.Entities;
using Domain.Enum;

namespace Application.Interfaces
{
    public interface IWorkoutService
    {
        Task<Workout> CreateWorkout(Workout workout);
        Task<List<Workout>> GetAllWorkouts();
        Task<List<Workout>> GetByUserId(Guid userId);
        Task<List<Workout>> GetByPlanId(Guid planId);
        Task<List<Workout>> GetTodayWorkouts(User user);
        Task<List<Workout>> GetCompletedWorkouts(User user);
        Task<bool> MarkAsCompleted(Guid workoutId, TimeSpan duration);
        Task<bool> UpdateWorkout(Workout workout);
        Task<bool> DeleteWorkout(Guid id);
        Task<int> GetTodayCompletedCount(User user);
    }
}
