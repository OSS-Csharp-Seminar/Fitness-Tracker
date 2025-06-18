using Domain.Entities;
using Domain.Interfaces;
using Domain.Enum;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkoutCatalogRepository : BaseRepository<WorkoutCatalog>, IWorkoutCatalogRepository
    {
        public WorkoutCatalogRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<WorkoutCatalog> GetByIdAsync(Guid id)
        {
            return await _context.WorkoutCatalogs
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<WorkoutCatalog>> GetAllAsync()
        {
            return await _context.WorkoutCatalogs
                .OrderBy(x => x.WorkoutName)
                .ToListAsync();
        }

        public async Task<IQueryable<WorkoutCatalog>> GetByWorkoutTypeAsync(List<WorkoutType> preferences)
        {
            var query = _context.WorkoutCatalogs
                .Where(x => x.tag.Any(tag => preferences.Contains(tag)))
                .OrderBy(x => x.WorkoutName);
            return await Task.FromResult(query);
        }

        public async Task<IQueryable<WorkoutCatalog>> SearchByNameAsync(string searchTerm)
        {
            var query = _context.WorkoutCatalogs
                .Where(x => x.WorkoutName.Contains(searchTerm) || x.Description.Contains(searchTerm))
                .OrderBy(x => x.WorkoutName);

            return await Task.FromResult(query);
        }

    }
}