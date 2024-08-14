using System.Collections.Generic;
using GymManagementSystem.Models;

namespace GymManagementSystem.Services
{
    public interface ITrainerService
    {
        List<Trainer> GetAllTrainers();
        void AddTrainer(Trainer trainer);
        void UpdateTrainer(Trainer trainer);
        void DeleteTrainer(int trainerID);

        Trainer GetTrainerById(int trainerId);
    }
}
