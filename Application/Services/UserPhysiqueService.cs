using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserPhysiqueService : IUserPhysiqueService
    {
        private readonly IUserPhysiqueRepository _userPhysiqueRepository;

        public UserPhysiqueService(IUserPhysiqueRepository userPhysiqueRepository)
        {
            _userPhysiqueRepository = userPhysiqueRepository;
        }

        public async Task<UserPhysique> CreateUserPhysique(UserPhysique userPhysique)
        {
            if (userPhysique == null)
                throw new ArgumentNullException(nameof(userPhysique));

            if (userPhysique.UserId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            if (userPhysique.Weight <= 0)
                throw new ArgumentException("Weight must be greater than 0");

            if (userPhysique.Date == default)
                userPhysique.Date = DateOnly.FromDateTime(DateTime.Now);

            var createdPhysique = await _userPhysiqueRepository.AddAsync(userPhysique);

            return createdPhysique;
        }

        public async Task<decimal> GetWeightProgress(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            return await _userPhysiqueRepository.GetWeightProgressAsync(userId);
        }

        public async Task<List<UserPhysique>> GetAllPhysiques()
        {
            var physiques = await _userPhysiqueRepository.GetAllAsync();
            return physiques.ToList();
        }

        public async Task<List<UserPhysique>> GetByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            var query = await _userPhysiqueRepository.GetByUserId(userId);
            return await query.ToListAsync();
        }

        public async Task<UserPhysique> GetLatestPhysique(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            return await _userPhysiqueRepository.GetLatestByUserIdAsync(userId);
        }

        public async Task<bool> UpdatePhysique(UserPhysique userPhysique)
        {
            if (userPhysique == null)
                throw new ArgumentNullException(nameof(userPhysique));

            if (userPhysique.Id == Guid.Empty)
                throw new ArgumentException("Physique ID is required");

            if (userPhysique.Weight <= 0)
                throw new ArgumentException("Weight must be greater than 0");

            return await _userPhysiqueRepository.UpdateAsync(userPhysique);
        }

        public async Task<bool> DeletePhysique(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Physique ID is required");

            return await _userPhysiqueRepository.DeleteAsync(id);
        }

        public async Task<UserPhysique> AddWeightMeasurement(User user, float weight, int? bmi = null)
        {
            var physique = new UserPhysique
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Weight = weight,
                BMI = bmi.HasValue ? bmi.Value : CalculateBMI(weight, user),
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            return await CreateUserPhysique(physique);
        }

        public async Task<string> GetProgressSummary(Guid userId)
        {
            try
            {
                var progress = await GetWeightProgress(userId);

                if (progress == 0)
                    return "No weight change recorded or insufficient data.";

                if (progress > 0)
                    return $"Weight increased by {progress:F1}kg since last measurement.";

                return $"Weight decreased by {Math.Abs(progress):F1}kg since last measurement.";
            }
            catch (Exception)
            {
                return "Unable to calculate weight progress.";
            }
        }

        public int CalculateBMI(float weight, User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (weight <= 0)
                throw new ArgumentException("Weight must be greater than 0");

            if (user.Height <= 0)
                throw new ArgumentException("User height must be greater than 0");

            // BMI formula: weight (kg) / height (m)²
            float heightInMeters = user.Height / 100f;
            float bmi = weight / (heightInMeters * heightInMeters);

            return (int)Math.Round(bmi);
        }

        public async Task<string> GetWeightProgressMessage(User user)
        {
            var weightLoss = await GetWeightProgress(user.Id);
            if (weightLoss < 0)
            {
                var absoluteLoss = Math.Abs((int)weightLoss);
                return absoluteLoss switch
                {
                    1 => "Amazing! You're 1 kg down from your starting weight! Keep up the fantastic work!",
                    2 => "Incredible progress! You've lost 2 kg already! Your dedication is paying off!",
                    3 => "Wow! 3 kg down and counting! You're becoming unstoppable!",
                    4 => "Outstanding! 4 kg lighter and feeling stronger! Your hard work shows!",
                    5 => "Phenomenal! You've crushed 5 kg! You're a true fitness warrior!",
                    6 => "Unbelievable! 6 kg down - you're on fire! Nothing can stop you now!",
                    7 => "Spectacular! 7 kg lost - you're transforming! Your future self thanks you!",
                    8 => "Magnificent! 8 kg down - you're a champion! Keep conquering those goals!",
                    9 => "Extraordinary! 9 kg lighter - you're unstoppable! The results speak for themselves!",
                    10 => "LEGENDARY! 10 kg down - you're a fitness legend! This is just the beginning!",
                    11 => "Mind-blowing! 11 kg lost - you're rewriting your story! Keep pushing boundaries!",
                    12 => "Incredible! 12 kg down - you're a transformation machine! Your discipline is inspiring!",
                    13 => "Phenomenal! 13 kg lighter - you're crushing every goal! Success looks amazing on you!",
                    14 => "Outstanding! 14 kg down - you're a true inspiration! Your journey motivates others!",
                    15 => "AMAZING! 15 kg lost - you're halfway to greatness! Your persistence is unmatched!",
                    16 => "Spectacular! 16 kg down - you're defying all limits! Keep soaring higher!",
                    17 => "Magnificent! 17 kg lighter - you're a force of nature! Nothing can hold you back!",
                    18 => "Extraordinary! 18 kg lost - you're redefining possible! Your willpower is legendary!",
                    19 => "Unbelievable! 19 kg down - you're almost at 20! The finish line is in sight!",
                    20 => "INCREDIBLE! 20 kg lost - you're a FITNESS LEGEND! You've conquered the impossible!",
                    21 => "Mind-blowing! 21 kg down - you're writing fitness history! Your story inspires millions!",
                    22 => "Phenomenal! 22 kg lighter - you're a transformation titan! Power through to the end!",
                    23 => "Outstanding! 23 kg lost - you're unstoppable force! Ride this wave to victory!",
                    24 => "Spectacular! 24 kg down - you're almost at 25! One more milestone awaits!",
                    25 => "LEGENDARY! 25 kg lost - QUARTER CENTURY! You've built a new you from the ground up!",
                    26 => "Incredible! 26 kg down - you're beyond amazing! Your dedication knows no bounds!",
                    27 => "Magnificent! 27 kg lighter - you're a miracle worker! Magic happens through hard work!",
                    28 => "Extraordinary! 28 kg lost - you're approaching 30! The summit is within reach!",
                    29 => "Unbelievable! 29 kg down - one away from 30! You're about to make history!",
                    30 => "PHENOMENAL! 30 kg lost - YOU'RE A FITNESS DEITY! You've transcended all expectations!",
                    31 => "Mind-blowing! 31 kg down - you're rewriting reality! Your transformation is cosmic!",
                    32 => "Incredible! 32 kg lighter - you're a walking miracle! Your journey inspires the world!",
                    33 => "Outstanding! 33 kg lost - you're defying gravity! Nothing is impossible for you!",
                    34 => "Spectacular! 34 kg down - you're approaching 35! Another milestone beckons!",
                    35 => "LEGENDARY! 35 kg lost - you're a FITNESS EMPEROR! Your empire of health grows stronger!",
                    36 => "Magnificent! 36 kg lighter - you're beyond human! Your superpower is determination!",
                    37 => "Extraordinary! 37 kg down - you're approaching 40! The peak awaits your conquest!",
                    38 => "Unbelievable! 38 kg lost - two away from 40! Thunder through to your goal!",
                    39 => "Phenomenal! 39 kg down - ONE AWAY FROM 40! History is about to be made!",
                    40 => "INCREDIBLE! 40 kg lost - YOU'RE A FITNESS GOD! You've achieved the impossible dream!",
                    41 => "Mind-blowing! 41 kg down - you're beyond legendary! Your story will inspire generations!",
                    42 => "Outstanding! 42 kg lighter - you're the answer to everything! 42 is the ultimate number!",
                    43 => "Spectacular! 43 kg lost - you're approaching the final frontier! 45 awaits your arrival!",
                    44 => "Magnificent! 44 kg down - one away from 45! Lightning is about to strike!",
                    45 => "LEGENDARY! 45 kg lost - ALMOST AT THE SUMMIT! The peak of achievement calls your name!",
                    46 => "Extraordinary! 46 kg lighter - you're in the stratosphere! Space can't contain your success!",
                    47 => "Unbelievable! 47 kg down - approaching the ultimate! Three more to the promised land!",
                    48 => "Phenomenal! 48 kg lost - two away from 50! The golden number awaits!",
                    49 => "INCREDIBLE! 49 kg down - ONE AWAY FROM 50! You're about to enter fitness royalty!",
                    50 => "ABSOLUTE LEGEND! 50 kg lost - YOU'RE A FITNESS IMMORTAL! You've achieved what mortals dream of!",
                    >= 51 => $"BEYOND LEGENDARY! {absoluteLoss} kg lost - YOU'VE TRANSCENDED FITNESS! You're no longer human, you're pure inspiration!"
                };
            }
            else if (weightLoss > 0)
            {
                var weightGain = (int)weightLoss;
                return weightGain switch
                {
                    <= 2 => "Hey champion! Time to turn that scale around! Your fitness journey starts with the first step!",
                    <= 5 => "No worries, warrior! Every champion faces challenges! Let's crush those goals together!",
                    <= 10 => "Rise up, future legend! Your comeback story starts today! Time to show the world your power!",
                    _ => "Transform mode: ACTIVATED! You have unlimited potential! Let's unleash the beast within and conquer!"
                };
            }     
           return "Steady as a rock! Consistency is key! Keep pushing forward, great things are coming!";
        }

        // NOVE METODE
        public async Task<int> GetCurrentBMI(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            var latestPhysique = await GetLatestPhysique(userId);
            if (latestPhysique == null)
                return 0;

            return latestPhysique.BMI;
        }

        public async Task<float> GetCurrentWeight(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            var latestPhysique = await GetLatestPhysique(userId);
            if (latestPhysique == null)
                return 0;

            return latestPhysique.Weight;
        }
    }
}