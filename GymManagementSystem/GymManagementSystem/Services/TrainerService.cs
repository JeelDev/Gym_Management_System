using GymManagementSystem.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace GymManagementSystem.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly string connectionString = "Server=localhost;Database=gym_management;User ID=root;Password=;Pooling=false;";

        public void AddTrainer(Trainer trainer)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            using var command = new MySqlCommand(
                "INSERT INTO Trainer (Name, Age, Gender, Email, Phone, Salary) VALUES (@Name, @Age, @Gender, @Email, @Phone, @Salary)",
                connection);
            command.Parameters.AddWithValue("@Name", trainer.Name);
            command.Parameters.AddWithValue("@Age", trainer.Age);
            command.Parameters.AddWithValue("@Gender", trainer.Gender);
            command.Parameters.AddWithValue("@Email", trainer.Email);
            command.Parameters.AddWithValue("@Phone", trainer.Phone);
            command.Parameters.AddWithValue("@Salary", trainer.Salary);
            command.ExecuteNonQuery();
        }

        public void UpdateTrainer(Trainer trainer)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            using var command = new MySqlCommand(
                "UPDATE Trainer SET Name = @Name, Age = @Age, Gender = @Gender, Email = @Email, Phone = @Phone, Salary = @Salary WHERE TrainerID = @TrainerID",
                connection);
            command.Parameters.AddWithValue("@Name", trainer.Name);
            command.Parameters.AddWithValue("@Age", trainer.Age);
            command.Parameters.AddWithValue("@Gender", trainer.Gender);
            command.Parameters.AddWithValue("@Email", trainer.Email);
            command.Parameters.AddWithValue("@Phone", trainer.Phone);
            command.Parameters.AddWithValue("@Salary", trainer.Salary);
            command.Parameters.AddWithValue("@TrainerID", trainer.TrainerID);
            command.ExecuteNonQuery();
        }

        public void DeleteTrainer(int trainerID)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            using var command = new MySqlCommand("DELETE FROM Trainer WHERE TrainerID = @TrainerID", connection);
            command.Parameters.AddWithValue("@TrainerID", trainerID);
            command.ExecuteNonQuery();
        }

        public List<Trainer> GetAllTrainers()
        {
            var trainers = new List<Trainer>();
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            using var command = new MySqlCommand("SELECT * FROM Trainer", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                trainers.Add(new Trainer
                {
                    TrainerID = reader.GetInt32("TrainerID"),
                    Name = reader.GetString("Name"),
                    Age = reader.GetInt32("Age"),
                    Gender = reader.GetString("Gender"),
                    Email = reader.GetString("Email"),
                    Phone = reader.GetString("Phone"),
                    Salary = reader.GetDecimal("Salary")
                });
            }
            return trainers;
        }

        public Trainer GetTrainerById(int trainerId)
        {
            Trainer trainer = null;
            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = "SELECT * FROM Trainer WHERE TrainerID = @TrainerID";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@TrainerID", trainerId);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    trainer = new Trainer
                    {
                        TrainerID = reader.GetInt32("TrainerID"),
                        Name = reader.GetString("Name"),
                        Age = reader.GetInt32("Age"),
                        Gender = reader.GetString("Gender"),
                        Email = reader.GetString("Email"),
                        Phone = reader.GetString("Phone"),
                        Salary = reader.GetDecimal("Salary")
                    };
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL-specific exceptions
                throw new Exception("Database error retrieving trainer: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                throw new Exception("Error retrieving trainer: " + ex.Message);
            }
            return trainer;
        }
    }
}
