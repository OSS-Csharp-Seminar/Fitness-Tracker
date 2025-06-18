using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Application.Services
{
    public class WorkoutCatalogService : IWorkoutCatalogService
    {
        private readonly IWorkoutCatalogRepository _workoutCatalogRepository;

        public WorkoutCatalogService(IWorkoutCatalogRepository workoutCatalogRepository)
        {
            _workoutCatalogRepository = workoutCatalogRepository;
        }

        public async Task<List<WorkoutCatalog>> GetAllExercises()
        {
            var exercises = await _workoutCatalogRepository.GetAllAsync();
            return exercises.ToList();
        }

        public async Task<WorkoutCatalog> GetExerciseById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Exercise ID is required");

            return await _workoutCatalogRepository.GetByIdAsync(id);
        }

        public async Task<List<WorkoutCatalog>> GetByWorkoutType(List<WorkoutType> preferences)
        {
            if (preferences == null || !preferences.Any())
                return await GetAllExercises();

            var query = await _workoutCatalogRepository.GetByWorkoutTypeAsync(preferences);
            return await query.ToListAsync();
        }

        public async Task<List<WorkoutCatalog>> SearchExercises(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllExercises();

            var query = await _workoutCatalogRepository.SearchByNameAsync(searchTerm.Trim());
            return await query.ToListAsync();
        }
    }
}