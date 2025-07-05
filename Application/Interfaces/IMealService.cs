using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMealService
    {
        Task<Meal> CreateMeal(Meal meal);
        Task<List<Meal>> GetByUserId(Guid userId);
        Task<List<Meal>> GetByDate(Guid userId, DateOnly date);
        Task<int> GetTotalCaloriesForDate(Guid userId, DateOnly date);
        Task<bool> UpdateMeal(Meal meal);
        Task<bool> DeleteMeal(Guid id);
    }
}
