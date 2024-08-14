using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using GymManagementSystem.Models;

namespace GymManagementSystem.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly string _connectionString = "Server=localhost;Database=gym_management;User ID=root;Password=;Pooling=false;";

        public void AddCustomer(Customer customer)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Customers (Name, Age, Gender, Email, Phone) VALUES (@Name, @Age, @Gender, @Email, @Phone)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Age", customer.Age);
                    cmd.Parameters.AddWithValue("@Gender", customer.Gender);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding customer: " + ex.Message);
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Customers SET Name = @Name, Age = @Age, Gender = @Gender, Email = @Email, Phone = @Phone WHERE CustomerID = @CustomerID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Age", customer.Age);
                    cmd.Parameters.AddWithValue("@Gender", customer.Gender);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                    cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating customer: " + ex.Message);
            }
        }

        public void DeleteCustomer(int customerId)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting customer: " + ex.Message);
            }
        }

        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Customers";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                CustomerID = reader.GetInt32("CustomerID"),
                                Name = reader.GetString("Name"),
                                Age = reader.GetInt32("Age"),
                                Gender = reader.GetString("Gender"),
                                Email = reader.GetString("Email"),
                                Phone = reader.GetString("Phone")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving customers: " + ex.Message);
            }
            return customers;
        }

        public Customer GetCustomerById(int customerId)
        {
            Customer customer = null;
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer = new Customer
                            {
                                CustomerID = reader.GetInt32("CustomerID"),
                                Name = reader.GetString("Name"),
                                Age = reader.GetInt32("Age"),
                                Gender = reader.GetString("Gender"),
                                Email = reader.GetString("Email"),
                                Phone = reader.GetString("Phone")
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving customer: " + ex.Message);
            }
            return customer;
        }
    }
}
