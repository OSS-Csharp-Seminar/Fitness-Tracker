using Domain.Entities;
using Domain.Enum;

namespace Application.Interfaces
{
    public interface IWorkoutCatalogService
    {
        Task<List<WorkoutCatalog>> GetAllExercises();
        Task<WorkoutCatalog> GetExerciseById(Guid id);
        Task<List<WorkoutCatalog>> GetByWorkoutType(List<WorkoutType> preferences);
        Task<List<WorkoutCatalog>> SearchExercises(string searchTerm);
    }
}
