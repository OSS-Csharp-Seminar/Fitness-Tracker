using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserPhysiqueService
    {
        Task<UserPhysique> CreateUserPhysique(UserPhysique userPhysique);
        Task<decimal> GetWeightProgress(Guid userId);
        Task<List<UserPhysique>> GetAllPhysiques();
        Task<List<UserPhysique>> GetByUserId(Guid userId);
        Task<UserPhysique> GetLatestPhysique(Guid userId);
        Task<bool> UpdatePhysique(UserPhysique userPhysique);
        Task<bool> DeletePhysique(Guid id);
        Task<UserPhysique> AddWeightMeasurement(User user, float weight, int? bmi = null);
        Task<string> GetProgressSummary(Guid userId);
        int CalculateBMI(float weight, User user);
        Task<string> GetWeightProgressMessage(User user);

        Task<int> GetCurrentBMI(Guid userId);
        Task<float> GetCurrentWeight(Guid userId);
    }
}