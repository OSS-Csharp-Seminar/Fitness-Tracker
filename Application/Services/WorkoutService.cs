using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public async Task<Workout> GetWorkoutById(Guid id)
        {
            if(id == Guid.Empty)
               throw new ArgumentNullException("id");

            return await _workoutRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Workout>> GetAllWorkouts()
        {
            var workouts = await _workoutRepository.AllAsync();

            return workouts;
        }

        public async Task<IEnumerable<Workout>> GetWorkoutByTag(string tag)
        {
            if (tag == null)
                throw new ArgumentNullException("The tag is empty!");

            return await _workoutRepository.GetByTagAsync(tag);
        }
    }
}
