using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMealRepository : IBaseRepository<Meal>
    {
        Task<IQueryable<Meal>> GetByUserIdAsync(Guid userId);
        Task<IQueryable<Meal>> GetByDateAsync(Guid userId, DateOnly date);
        Task<IQueryable<Meal>> GetAllByUserAsync(Guid userId);
        Task<int> GetTotalCaloriesForDateAsync(Guid userId, DateOnly date);
    }
}
