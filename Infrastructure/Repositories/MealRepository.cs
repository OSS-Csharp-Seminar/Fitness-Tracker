using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MealRepository : BaseRepository<Meal>, IMealRepository
    {
        public MealRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<Meal>> GetByUserIdAsync(Guid userId)
        {
            var query = _context.Meals
                .Where(m => m.UserId == userId)
                .AsQueryable();

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<Meal>> GetByDateAsync(Guid userId, DateOnly date)
        {
            var query = _context.Meals
                .Where(m => m.UserId == userId && EF.Property<DateTime>(m, "CreatedAt").Date == date.ToDateTime(TimeOnly.MinValue).Date)
                .AsQueryable();

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<Meal>> GetAllByUserAsync(Guid userId)
        {
            var query = _context.Meals
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => EF.Property<DateTime>(m, "CreatedAt"))
                .AsQueryable();

            return await Task.FromResult(query);
        }

        public async Task<int> GetTotalCaloriesForDateAsync(Guid userId, DateOnly date)
        {
            var total = await _context.Meals
                .Where(m => m.UserId == userId && EF.Property<DateTime>(m, "CreatedAt").Date == date.ToDateTime(TimeOnly.MinValue).Date)
                .SumAsync(m => m.Calories);

            return total;
        }
    }
}
