using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enum;

namespace Domain.Interfaces
{
    public interface IWorkoutCatalogRepository : IBaseRepository<WorkoutCatalog>
    {
        Task<IEnumerable<WorkoutCatalog>> GetByWorkoutTypeAsync(List<WorkoutType> preferences);
        Task<IQueryable<WorkoutCatalog>> SearchByNameAsync(string searchTerm);
    }
}
