using GymManagementSystem.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace GymManagementSystem.Services
{
    public class TrainingSessionService : ITrainingSessionService
    {
        private readonly string connectionString = "Server=localhost;Database=gym_management;User ID=root;Password=;Pooling=false;";

        public void AddSession(TrainingSession session)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            using var command = new MySqlCommand(
                "INSERT INTO TrainingSession (CustomerID, TrainerID, SessionTiming, TrainingStartDate, TrainingEndDate, SessionStatus) " +
                "VALUES (@CustomerID, @TrainerID, @SessionTiming, @TrainingStartDate, @TrainingEndDate, @SessionStatus)",
                connection);
            command.Parameters.AddWithValue("@CustomerID", session.CustomerID);
            command.Parameters.AddWithValue("@TrainerID", session.TrainerID);
            command.Parameters.AddWithValue("@SessionTiming", session.SessionTiming.ToString(@"hh\:mm\:ss")); // Convert TimeSpan to string format
            command.Parameters.AddWithValue("@TrainingStartDate", session.TrainingStartDate);
            command.Parameters.AddWithValue("@TrainingEndDate", session.TrainingEndDate);
            command.Parameters.AddWithValue("@SessionStatus", session.SessionStatus);
            command.ExecuteNonQuery();
        }

        public void UpdateSession(TrainingSession session)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            using var command = new MySqlCommand(
                "UPDATE TrainingSession SET CustomerID = @CustomerID, TrainerID = @TrainerID, SessionTiming = @SessionTiming, " +
                "TrainingStartDate = @TrainingStartDate, TrainingEndDate = @TrainingEndDate, SessionStatus = @SessionStatus " +
                "WHERE SessionID = @SessionID",
                connection);
            command.Parameters.AddWithValue("@CustomerID", session.CustomerID);
            command.Parameters.AddWithValue("@TrainerID", session.TrainerID);
            command.Parameters.AddWithValue("@SessionTiming", session.SessionTiming.ToString(@"hh\:mm\:ss")); // Convert TimeSpan to string format
            command.Parameters.AddWithValue("@TrainingStartDate", session.TrainingStartDate);
            command.Parameters.AddWithValue("@TrainingEndDate", session.TrainingEndDate);
            command.Parameters.AddWithValue("@SessionStatus", session.SessionStatus);
            command.Parameters.AddWithValue("@SessionID", session.SessionID);
            command.ExecuteNonQuery();
        }

        public void DeleteSession(int sessionID)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            using var command = new MySqlCommand("DELETE FROM TrainingSession WHERE SessionID = @SessionID", connection);
            command.Parameters.AddWithValue("@SessionID", sessionID);
            command.ExecuteNonQuery();
        }

        public IEnumerable<TrainingSession> GetAllSessions() // Match the interface return type
        {
            var sessions = new List<TrainingSession>();
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            using var command = new MySqlCommand("SELECT * FROM TrainingSession", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                sessions.Add(new TrainingSession
                {
                    SessionID = reader.GetInt32("SessionID"),
                    CustomerID = reader.GetInt32("CustomerID"),
                    TrainerID = reader.GetInt32("TrainerID"),
                    SessionTiming = TimeSpan.Parse(reader.GetString("SessionTiming")), // Convert string to TimeSpan
                    TrainingStartDate = reader.GetDateTime("TrainingStartDate"),
                    TrainingEndDate = reader.GetDateTime("TrainingEndDate"),
                    SessionStatus = reader.GetString("SessionStatus")
                });
            }
            return sessions;
        }
    }
}
