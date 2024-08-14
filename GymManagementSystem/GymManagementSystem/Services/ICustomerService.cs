using GymManagementSystem.Models;
using System.Collections.Generic;

namespace GymManagementSystem.Services
{
    public interface ICustomerService
    {
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int customerId);
        List<Customer> GetAllCustomers();
        Customer GetCustomerById(int customerId);
    }
}
