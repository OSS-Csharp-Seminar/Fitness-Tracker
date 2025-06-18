using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserPhysiqueRepository: IBaseRepository<UserPhysique>
    {
        Task<IQueryable<UserPhysique>> GetByUserId(Guid id);
        Task<UserPhysique> GetLatestAsync(User user);
        Task<decimal> GetWeightProgressAsync(Guid userId, int days = 30);

    }
}
