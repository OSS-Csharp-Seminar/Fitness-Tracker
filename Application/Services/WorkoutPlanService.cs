using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;

namespace Application.Services
{
    internal class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;

        public WorkoutPlanService(IWorkoutPlanRepository workoutPlanRepository)
        {
            _workoutPlanRepository = workoutPlanRepository;
        }

        public async Task<WorkoutPlan> GetWorkoutPlan(WorkoutPlan workoutPlan)
        {
            if (workoutPlan == null)
                throw new ArgumentException("WorkoutPlan cannot be null");
            if (workoutPlan.Id == Guid.Empty)
                workoutPlan.Id = Guid.NewGuid();

            return await _workoutPlanRepository.AddAsync(workoutPlan);
        }

        public async Task<WorkoutPlan> GetByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User Id not exists!");

            var workutPlan = await _workoutPlanRepository.GetByUserIdAsync(userId);

            if (workutPlan == null)
                throw new ArgumentException($"The User whit {userId} dont have plan");

            return workutPlan;
        }

        public async Task<bool> UpdateWorkoutPlan(WorkoutPlan workoutPlan)
        {
            try
            {
                if (workoutPlan == null)
                    throw new ArgumentNullException(nameof(workoutPlan));

                if (workoutPlan.Id == Guid.Empty)
                    throw new ArgumentException("WorkoutPlan ID ne može biti prazan", nameof(workoutPlan));

                var existingWorkoutPlan = await _workoutPlanRepository.GetByIdAsync(workoutPlan.Id);
                if (existingWorkoutPlan == null)
                    return false;

                if (workoutPlan.MaxCalories != 0)
                    existingWorkoutPlan.MaxCalories = workoutPlan.MaxCalories;

                if (workoutPlan.BMI != 0)
                    existingWorkoutPlan.BMI = workoutPlan.BMI;

                if (workoutPlan.WorkoutPlanType != default(WorkoutPlanType))
                    existingWorkoutPlan.WorkoutPlanType = workoutPlan.WorkoutPlanType;

                if (workoutPlan.TrainingDuration != TimeOnly.FromDateTime(DateTime.MinValue))
                    existingWorkoutPlan.TrainingDuration = workoutPlan.TrainingDuration;

                if (workoutPlan.TrainingDays != 0)
                    existingWorkoutPlan.TrainingDays = workoutPlan.TrainingDays;

                if (workoutPlan.TargetWeight != 0)
                    existingWorkoutPlan.TargetWeight = workoutPlan.TargetWeight;

                return await _workoutPlanRepository.UpdateAsync(existingWorkoutPlan);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<WorkoutPlan>> GetAllUserWorkoutPlans(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                    throw new ArgumentException("UserId can not be emty", nameof(userId));

                var workoutPlans = await _workoutPlanRepository.GetAllAsync(userId);

                return workoutPlans;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"erroe handling: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("ID ne može biti prazan", nameof(id));

                if (userId == Guid.Empty)
                    throw new ArgumentException("UserId ne može biti prazan", nameof(userId));

                var userWorkoutPlans = await _workoutPlanRepository.GetAllAsync(userId);

                var planToDelete = userWorkoutPlans.FirstOrDefault(wp => wp.Id == id);

                if (planToDelete == null)
                    throw new ArgumentException("The workout plan does not exist for this user!");

                return await _workoutPlanRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
