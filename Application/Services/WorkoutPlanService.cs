using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;
        private readonly IWorkoutService _workoutService;
        private readonly IWorkoutCatalogService _workoutCatalogService;
        private readonly Random _random;

        public WorkoutPlanService(
            IWorkoutPlanRepository workoutPlanRepository,
            IWorkoutService workoutService,
            IWorkoutCatalogService workoutCatalogService)
        {
            _workoutPlanRepository = workoutPlanRepository;
            _workoutService = workoutService;
            _workoutCatalogService = workoutCatalogService;
            _random = new Random();
        }

        public async Task<WorkoutPlan> CreateWorkoutPlan(WorkoutPlan workoutPlan)
        {
            if (workoutPlan == null)
                throw new ArgumentNullException(nameof(workoutPlan));

            if (workoutPlan.UserId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            if (workoutPlan.TrainingDays <= 0 || workoutPlan.TrainingDays > 7)
                throw new ArgumentException("Training days must be between 1 and 7");

            if (workoutPlan.TargetWeight <= 0)
                throw new ArgumentException("Target weight must be greater than 0");

            workoutPlan.Id = Guid.NewGuid();
            workoutPlan.StartDate = DateTime.Now;

            return await _workoutPlanRepository.AddAsync(workoutPlan);
        }

        public async Task<WorkoutPlan> GetWorkoutPlanById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Workout plan ID is required");

            return await _workoutPlanRepository.GetByIdAsync(id);
        }

        public async Task<List<WorkoutPlan>> GetByUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var query = await _workoutPlanRepository.GetByUserAsync(user);
            return await query.ToListAsync();
        }

        public async Task<bool> DeleteWorkoutPlan(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Workout plan ID is required");

            return await _workoutPlanRepository.DeleteAsync(id);
        }

        public async Task<List<Workout>> GenerateDailyWorkout(User user, Guid planId)
        {
            var workoutPlan = await _workoutPlanRepository.GetByIdAsync(planId);
            if (workoutPlan == null)
                throw new InvalidOperationException("Workout plan not found");

            if (workoutPlan.UserId != user.Id)
                throw new UnauthorizedAccessException("This workout plan does not belong to the user");

            if (!await ShouldGenerateNewWorkout(user))
                return await _workoutService.GetTodayWorkouts(user);

            var exercises = await GetSuitableExercises(workoutPlan);
            var selectedExercises = SelectExercisesForWorkout(exercises, workoutPlan);

            var workouts = new List<Workout>();
            var today = DateOnly.FromDateTime(DateTime.Now);

            foreach (var exercise in selectedExercises)
            {
                var workout = new Workout
                {
                    PlanId = workoutPlan.Id,
                    CatalogId = exercise.Id,
                    Date = today,
                    Duration = TimeSpan.Zero
                };

                workouts.Add(await _workoutService.CreateWorkout(workout));
            }

            return workouts;
        }

        public async Task<List<Workout>> GetWorkoutHistory(User user, DateOnly? fromDate = null, DateOnly? toDate = null)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var workouts = await _workoutService.GetCompletedWorkouts(user);

            if (fromDate.HasValue)
                workouts = workouts.Where(w => w.Date >= fromDate.Value).ToList();

            if (toDate.HasValue)
                workouts = workouts.Where(w => w.Date <= toDate.Value).ToList();

            return workouts.OrderByDescending(w => w.Date).ToList();
        }

        public async Task<bool> ShouldGenerateNewWorkout(User user)
        {
            var todayWorkouts = await _workoutService.GetTodayWorkouts(user);

            if (!todayWorkouts.Any())
                return true;

            if (todayWorkouts.Any(w => w.Completed))
            {
                var lastCompletedDate = todayWorkouts.Max(w => w.Date);
                var today = DateOnly.FromDateTime(DateTime.Now);
                return lastCompletedDate < today;
            }

            return false;
        }

        public async Task<int> CalculateExpectedDuration(WorkoutPlan workoutPlan)
        {
            if (workoutPlan == null)
                throw new ArgumentNullException(nameof(workoutPlan));

            return workoutPlan.TrainingDuration.Hour * 60 + workoutPlan.TrainingDuration.Minute;
        }

        private async Task<List<WorkoutCatalog>> GetSuitableExercises(WorkoutPlan workoutPlan)
        {
            if (workoutPlan.WorkoutPreference.Any())
            {
                return await _workoutCatalogService.GetByWorkoutType(workoutPlan.WorkoutPreference);
            }

            return await _workoutCatalogService.GetAllExercises();
        }

        private List<WorkoutCatalog> SelectExercisesForWorkout(List<WorkoutCatalog> availableExercises, WorkoutPlan workoutPlan)
        {
            if (!availableExercises.Any())
                return new List<WorkoutCatalog>();

            var targetDurationMinutes = CalculateExpectedDuration(workoutPlan).Result;
            const int maxExerciseDuration = 3;
            var numberOfExercises = (int)Math.Ceiling((double)targetDurationMinutes / maxExerciseDuration);
            numberOfExercises = Math.Max(2, Math.Min(20, numberOfExercises));

            var selectedExercises = availableExercises
                .OrderBy(x => _random.Next())
                .Take(numberOfExercises)
                .ToList();

            var totalCaloriesBurned = CalculateTotalCalories(selectedExercises, targetDurationMinutes, numberOfExercises);

            return selectedExercises;
        }

        private int CalculateTotalCalories(List<WorkoutCatalog> exercises, int totalDurationMinutes, int numberOfExercises)
        {
            var minutesPerExercise = (double)totalDurationMinutes / numberOfExercises;
            var totalCalories = 0;

            foreach (var exercise in exercises)
            {
                totalCalories += (int)(exercise.CaloriesBurned * minutesPerExercise);
            }

            return totalCalories;
        }
    }
}