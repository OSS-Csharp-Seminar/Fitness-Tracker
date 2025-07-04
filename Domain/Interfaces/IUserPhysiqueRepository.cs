using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserPhysiqueRepository : IBaseRepository<UserPhysique>
    {
        Task<IQueryable<UserPhysique>> GetByUserId(Guid id);
        Task<UserPhysique> GetLatestAsync(User user);
        Task<UserPhysique> GetLatestByUserIdAsync(Guid userId); // NOVA METODA
        Task<decimal> GetWeightProgressAsync(Guid userId);
    }
}