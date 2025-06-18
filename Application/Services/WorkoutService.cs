using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public async Task<Workout> CreateWorkout(Workout workout)
        {
            if (workout == null)
                throw new ArgumentNullException(nameof(workout));

            if (workout.CatalogId == Guid.Empty)
                throw new ArgumentException("Catalog ID is required");

            if (workout.Date == default)
                workout.Date = DateOnly.FromDateTime(DateTime.Now);

            workout.Id = Guid.NewGuid();
            workout.Completed = false;

            return await _workoutRepository.AddAsync(workout);
        }

        public async Task<List<Workout>> GetAllWorkouts()
        {
            var workouts = await _workoutRepository.GetAllAsync();
            return workouts.ToList();
        }

        public async Task<List<Workout>> GetByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            var query = await _workoutRepository.GetByUserIdAsync(userId);
            return await query.ToListAsync();
        }

        public async Task<List<Workout>> GetByPlanId(Guid planId)
        {
            if (planId == Guid.Empty)
                throw new ArgumentException("Plan ID is required");

            var query = await _workoutRepository.GetByPlanIdAsync(planId);
            return await query.ToListAsync();
        }

        public async Task<List<Workout>> GetTodayWorkouts(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var query = await _workoutRepository.GetTodayWorkoutsAsync(user);
            return await query.ToListAsync();
        }

        public async Task<List<Workout>> GetCompletedWorkouts(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var query = await _workoutRepository.GetCompletedWorkoutsAsync(user);
            return await query.ToListAsync();
        }

        public async Task<bool> MarkAsCompleted(Guid workoutId, TimeSpan duration)
        {
            if (workoutId == Guid.Empty)
                throw new ArgumentException("Workout ID is required");

            var workout = await _workoutRepository.GetByIdAsync(workoutId);
            if (workout == null)
                return false;

            workout.Completed = true;
            workout.Duration = duration;

            return await _workoutRepository.UpdateAsync(workout);
        }

        public async Task<bool> UpdateWorkout(Workout workout)
        {
            if (workout == null)
                throw new ArgumentNullException(nameof(workout));

            if (workout.Id == Guid.Empty)
                throw new ArgumentException("Workout ID is required");

            return await _workoutRepository.UpdateAsync(workout);
        }

        public async Task<bool> DeleteWorkout(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Workout ID is required");

            return await _workoutRepository.DeleteAsync(id);
        }

        public async Task<int> GetTodayCompletedCount(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _workoutRepository.GetCompletedCountAsync(user, today);
        }
    }
}