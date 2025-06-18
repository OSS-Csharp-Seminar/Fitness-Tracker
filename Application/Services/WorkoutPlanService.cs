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

        //generira trening - GLAVNA METODA
        public async Task<List<Workout>> GetOrGenerateWorkoutsForToday(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var todayWorkouts = await _workoutService.GetTodayWorkouts(user);

            if (todayWorkouts.Any())
            {
                return todayWorkouts;
            }

            var userPlans = await GetByUser(user);
            var activePlan = userPlans.OrderByDescending(p => p.StartDate).FirstOrDefault();

            if (activePlan == null)
                return new List<Workout>(); 

            if (!IsTrainingDay(activePlan))
                return new List<Workout>(); 

            return await GenerateWorkoutsForToday(activePlan);
        }

        private async Task<List<Workout>> GenerateWorkoutsForToday(WorkoutPlan workoutPlan)
        {
            var exercises = await GetSuitableExercises(workoutPlan);
            var selectedExercises = SelectExercisesForWorkout(exercises, workoutPlan);

            var workouts = new List<Workout>();
            var today = DateOnly.FromDateTime(DateTime.Now);

            var totalDurationMinutes = CalculateExpectedDuration(workoutPlan);
            var durationPerExercise = TimeSpan.FromMinutes((double)totalDurationMinutes / Math.Max(1, selectedExercises.Count));

            foreach (var exercise in selectedExercises)
            {
                var workout = new Workout
                {
                    PlanId = workoutPlan.Id,
                    CatalogId = exercise.Id,
                    Date = today,
                    Duration = durationPerExercise
                };

                workouts.Add(await _workoutService.CreateWorkout(workout));
            }

            return workouts;
        }

        private bool IsTrainingDay(WorkoutPlan workoutPlan)
        {
            var today = DateTime.Now.DayOfWeek;
            var trainingDays = GetTrainingDaysOfWeek(workoutPlan.TrainingDays);
            return trainingDays.Contains(today);
        }

        private List<DayOfWeek> GetTrainingDaysOfWeek(int trainingDaysCount)
        {
            var days = new List<DayOfWeek>();

            switch (trainingDaysCount)
            {
                case 1:
                    days.Add(DayOfWeek.Monday);
                    break;
                case 2:
                    days.AddRange(new[] { DayOfWeek.Monday, DayOfWeek.Thursday });
                    break;
                case 3:
                    days.AddRange(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday });
                    break;
                case 4:
                    days.AddRange(new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                    break;
                case 5:
                    days.AddRange(new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                    break;
                case 6:
                    days.AddRange(new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday });
                    break;
                case 7:
                    days.AddRange(Enum.GetValues<DayOfWeek>());
                    break;
            }

            return days;
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

        public int CalculateExpectedDuration(WorkoutPlan workoutPlan)
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

            var targetDurationMinutes = CalculateExpectedDuration(workoutPlan);
            const int averageExerciseDurationMinutes = 5;
            var numberOfExercises = Math.Max(3, Math.Min(15, targetDurationMinutes / averageExerciseDurationMinutes));

            var selectedExercises = availableExercises
                .OrderBy(x => _random.Next())
                .Take(numberOfExercises)
                .ToList();

            return selectedExercises;
        }

        public int CalculateMaxCalories(float currentWeight, float targetWeight, string lifestyleType)
        {
            var baseBMR = lifestyleType switch
            {
                "sedentary" => 1600,
                "moderately_active" => 1800,
                "active" => 2000,
                _ => 1800
            };

            var weightDifference = currentWeight - targetWeight;
            int calorieAdjustment;

            if (weightDifference > 0) // Mršavljenje
            {
                if (weightDifference <= 5)
                    calorieAdjustment = -300;
                else if (weightDifference <= 15)
                    calorieAdjustment = -500;
                else
                    calorieAdjustment = -700;
            }
            else if (weightDifference < 0) // Debljanje
            {
                var weightToGain = Math.Abs(weightDifference);
                if (weightToGain <= 5)
                    calorieAdjustment = 300;
                else
                    calorieAdjustment = 500;
            }
            else // Održavanje
            {
                calorieAdjustment = 0;
            }

            return Math.Max(1200, baseBMR + calorieAdjustment);
        }
    }
}