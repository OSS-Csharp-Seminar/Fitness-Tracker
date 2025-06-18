using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserPhysiqueRepository : BaseRepository<UserPhysique>, IUserPhysiqueRepository
    {
        public UserPhysiqueRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<UserPhysique> GetByIdAsync(Guid id)
        {
            return await _context.UserPhysiques
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<UserPhysique>> GetAllAsync()
        {
            return await _context.UserPhysiques
                .Include(x => x.User)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
        public async Task<IQueryable<UserPhysique>> GetByUserId(Guid id)
        {
            var query = _context.UserPhysiques
                .Include(x => x.User)
                .Where(x => x.Id == id)
                .OrderByDescending(x => x.UserId);

            return await Task.FromResult(query);
        }

        public async Task<UserPhysique> GetLatestAsync(User user)
        {
            return await _context.UserPhysiques
                .Where(x => x.User == user)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetWeightProgressAsync(Guid userId, int days = 30)
        {
            var lastTwoDartes = await _context.UserPhysiques
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Date)
                .Take(2)
                .ToListAsync();

            var lastWeight = lastTwoDartes[0].Weight;
            var previousWeight = lastTwoDartes[1].Weight;

            return (decimal)(lastWeight - previousWeight);
        }
    }
}
