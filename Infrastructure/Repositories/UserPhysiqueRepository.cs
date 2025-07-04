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

        public async Task<IQueryable<UserPhysique>> GetByUserId(Guid userId)
        {
            var query = _context.UserPhysiques
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Date);

            return await Task.FromResult(query);
        }

        public async Task<UserPhysique> GetLatestAsync(User user)
        {
            return await _context.UserPhysiques
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync();
        }

        // NOVA METODA
        public async Task<UserPhysique> GetLatestByUserIdAsync(Guid userId)
        {
            return await _context.UserPhysiques
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetWeightProgressAsync(Guid userId)
        {
            var allRecords = await _context.UserPhysiques
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Date) 
                .ToListAsync();

            if (allRecords.Count < 2)
                return 0;

            var firstWeight = allRecords.First().Weight;  
            var lastWeight = allRecords.Last().Weight;    

            // Vraća razliku: zadnji - prvi (negativno = gubitak, pozitivno = dobitak)
            return (decimal)(lastWeight - firstWeight);
        }
    }
}