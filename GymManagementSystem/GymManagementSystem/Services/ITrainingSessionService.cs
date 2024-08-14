using GymManagementSystem.Models;
using System.Collections.Generic;

namespace GymManagementSystem.Services
{
    public interface ITrainingSessionService
    {
        // Retrieve all training sessions
        IEnumerable<TrainingSession> GetAllSessions();

        // Add a new training session
        void AddSession(TrainingSession session);

        // Update an existing training session
        void UpdateSession(TrainingSession session);

        // Delete a training session by its ID
        void DeleteSession(int sessionId);
    }
}
