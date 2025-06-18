using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkoutRepository : BaseRepository<Workout>, IWorkoutRepository
    {
        public WorkoutRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Workout> GetByIdAsync(Guid id)
        {
            return await _context.Workouts
                .Include(x => x.Plan)
                .Include(x => x.WorkoutCatalog)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<Workout>> GetAllAsync()
        {
            return await _context.Workouts
                .Include(x => x.Plan)
                .Include(x => x.WorkoutCatalog)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<IQueryable<Workout>> GetByUserIdAsync(Guid userId)
        {
            var query = _context.Workouts
                .Include(x => x.Plan)
                .Include(x => x.WorkoutCatalog)
                .Where(x => x.Plan.UserId == userId)
                .OrderByDescending(x => x.Date);

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<Workout>> GetByPlanIdAsync(Guid planId)
        {
            var query = _context.Workouts
                .Include(x => x.WorkoutCatalog)
                .Where(x => x.PlanId == planId)
                .OrderByDescending(x => x.Date);

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<Workout>> GetByDateAsync(DateOnly date)
        {
            var query = _context.Workouts
                .Include(x => x.Plan)
                .Include(x => x.WorkoutCatalog)
                .Where(x => x.Date == date)
                .OrderBy(x => x.WorkoutCatalog.WorkoutName);

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<Workout>> GetTodayWorkoutsAsync(User user)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var query = _context.Workouts
                .Include(x => x.Plan)
                .Include(x => x.WorkoutCatalog)
                .Where(x => x.Plan.UserId == user.Id && x.Date == today);

            return await Task.FromResult(query);
        }

        public async Task<IQueryable<Workout>> GetCompletedWorkoutsAsync(User user)
        {
            var query = _context.Workouts
                .Include(x => x.Plan)
                .Include(x => x.WorkoutCatalog)
                .Where(x => x.Plan.UserId == user.Id && x.Completed == true)
                .OrderByDescending(x => x.Date);

            return await Task.FromResult(query);
        }

        public async Task<int> GetCompletedCountAsync(User user, DateOnly date)
        {
            return await _context.Workouts
                .Where(x => x.Plan.UserId == user.Id && x.Date == date && x.Completed == true)
                .CountAsync();
        }
    }
}