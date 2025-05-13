using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IWorkoutService
    {
        Task<Workout> GetWorkoutById(Guid id);
        Task<IEnumerable<Workout>> GetAllWorkouts();
        Task<IEnumerable<Workout>> GetWorkoutByTag(string tag);
    }
}
