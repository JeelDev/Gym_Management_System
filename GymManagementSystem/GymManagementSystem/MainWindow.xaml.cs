using GymManagementSystem.Models;
using GymManagementSystem.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GymManagementSystem
{
    public partial class MainWindow : Window
    {
        private readonly ICustomerService _customerService;
        private readonly ITrainerService _trainerService;
        private readonly ITrainingSessionService _trainingSessionService;
        private Trainer _selectedTrainer;
        private TrainingSession _selectedSession;

        public MainWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
            _trainerService = new TrainerService();
            _trainingSessionService = new TrainingSessionService();
            LoadCustomers();
            LoadTrainers();
            LoadTrainingSessions();
        }

        private void LoadTrainingSessions()
        {
            dgTrainingSessions.ItemsSource = _trainingSessionService.GetAllSessions();
        }

        private void AddSession_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var session = new TrainingSession
                {
                    CustomerID = (int)cmbCustomerID.SelectedValue,
                    TrainerID = (int)cmbTrainerID.SelectedValue,
                    SessionTiming = TimeSpan.Parse(txtSessionTiming.Text),
                    TrainingStartDate = dpStartDate.SelectedDate.Value,
                    TrainingEndDate = dpEndDate.SelectedDate.Value,
                    SessionStatus = txtSessionStatus.Text
                };

                _trainingSessionService.AddSession(session);
                LoadTrainingSessions();
                ClearSessionForm();
                MessageBox.Show("Training session added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding training session: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Update Training Session
        private void UpdateSession_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedSession == null)
                {
                    MessageBox.Show("Please select a training session to update.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validate ComboBox selections
                if (cmbCustomerID.SelectedItem == null || cmbTrainerID.SelectedItem == null)
                {
                    MessageBox.Show("Please select both Customer and Trainer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Retrieve selected values from ComboBox
                _selectedSession.CustomerID = (int)cmbCustomerID.SelectedValue; // Assuming SelectedValue is an int
                _selectedSession.TrainerID = (int)cmbTrainerID.SelectedValue; // Assuming SelectedValue is an int

                // Validate and retrieve other input values
                if (!TimeSpan.TryParse(txtSessionTiming.Text, out TimeSpan sessionTiming))
                {
                    MessageBox.Show("Invalid session timing format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                _selectedSession.SessionTiming = sessionTiming;

                if (!dpStartDate.SelectedDate.HasValue || !dpEndDate.SelectedDate.HasValue)
                {
                    MessageBox.Show("Please select both start and end dates.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                _selectedSession.TrainingStartDate = dpStartDate.SelectedDate.Value;
                _selectedSession.TrainingEndDate = dpEndDate.SelectedDate.Value;

                _selectedSession.SessionStatus = txtSessionStatus.Text;

                // Update session and refresh UI
                _trainingSessionService.UpdateSession(_selectedSession);
                LoadTrainingSessions();
                ClearSessionForm();

                MessageBox.Show("Training session updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating training session: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Delete Training Session
        private void DeleteSession_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedSession == null)
                {
                    MessageBox.Show("Please select a training session to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _trainingSessionService.DeleteSession(_selectedSession.SessionID);
                LoadTrainingSessions();
                ClearSessionForm();
                MessageBox.Show("Training session deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting training session: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Handle selection of a session from DataGrid
        private void dgTrainingSessions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSession = dgTrainingSessions.SelectedItem as TrainingSession;

            if (_selectedSession != null)
            {
                // Set ComboBox selected values
                if (cmbCustomerID.ItemsSource != null)
                {
                    cmbCustomerID.SelectedValue = _selectedSession.CustomerID;
                }

                if (cmbTrainerID.ItemsSource != null)
                {
                    cmbTrainerID.SelectedValue = _selectedSession.TrainerID;
                }

                // Set other input values
                txtSessionTiming.Text = _selectedSession.SessionTiming.ToString(@"hh\:mm\:ss");
                dpStartDate.SelectedDate = _selectedSession.TrainingStartDate;
                dpEndDate.SelectedDate = _selectedSession.TrainingEndDate;
                txtSessionStatus.Text = _selectedSession.SessionStatus;
            }
        }


        // Clear Session Form
        private void ClearSessionForm()
{
    // Clear ComboBox selections
    cmbCustomerID.SelectedIndex = -1; // or set to null if using nullable types
    cmbTrainerID.SelectedIndex = -1; // or set to null if using nullable types

    // Clear other input fields
    txtSessionTiming.Clear();
    dpStartDate.SelectedDate = null;
    dpEndDate.SelectedDate = null;
    txtSessionStatus.Clear();

    // Reset selected session
    _selectedSession = null;
}



        private void LoadCustomers()
        {
            dgCustomers.ItemsSource = _customerService.GetAllCustomers();
            cmbCustomerID.ItemsSource = _customerService.GetAllCustomers();
            cmbCustomerID.SelectedIndex = 0; // Select the first item by default
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string email = txtEmail.Text;

                // Check if the email already exists
                var existingCustomers = _customerService.GetAllCustomers();
                if (existingCustomers.Any(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("A customer with this email already exists.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var customer = new Customer
                {
                    Name = txtName.Text,
                    Age = int.Parse(txtAge.Text),
                    Gender = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Email = email,
                    Phone = txtPhone.Text
                };

                _customerService.AddCustomer(customer);
                LoadCustomers();
                ClearFormFields();
                MessageBox.Show("Customer added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCustomers.SelectedItem is not Customer selectedCustomer)
                {
                    MessageBox.Show("Please select a customer to update.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selectedCustomer.Name = txtName.Text;
                selectedCustomer.Age = int.Parse(txtAge.Text);
                selectedCustomer.Gender = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString();
                selectedCustomer.Email = txtEmail.Text;
                selectedCustomer.Phone = txtPhone.Text; // Ensure phone number is updated

                _customerService.UpdateCustomer(selectedCustomer);
                LoadCustomers();
                MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCustomers.SelectedItem is not Customer selectedCustomer)
                {
                    MessageBox.Show("Please select a customer to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _customerService.DeleteCustomer(selectedCustomer.CustomerID);
                LoadCustomers();
                MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting customer: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ClearFormFields()
        {
            txtName.Text = string.Empty;
            txtAge.Text = string.Empty;
            cmbGender.SelectedIndex = -1;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
        }


        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer selectedCustomer)
            {
                txtName.Text = selectedCustomer.Name;
                txtAge.Text = selectedCustomer.Age.ToString();
                cmbGender.SelectedItem = cmbGender.Items.OfType<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == selectedCustomer.Gender);
                txtEmail.Text = selectedCustomer.Email;
                txtPhone.Text = selectedCustomer.Phone; // Populate phone number field
            }
        }

        private void LoadTrainers()
        {
            dgTrainers.ItemsSource = _trainerService.GetAllTrainers();
            cmbTrainerID.ItemsSource = _trainerService.GetAllTrainers();
            cmbTrainerID.SelectedIndex = 0; // Select the first item by default
        }

        private void AddTrainer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var trainer = new Trainer
                {
                    Name = txtTrainerName.Text,
                    Age = int.Parse(txtTrainerAge.Text),
                    Gender = (cmbTrainerGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Email = txtTrainerEmail.Text,
                    Phone = txtTrainerPhone.Text,
                    Salary = decimal.Parse(txtTrainerSalary.Text)
                };

                _trainerService.AddTrainer(trainer);
                LoadTrainers();
                ClearTrainerForm();
                MessageBox.Show("Trainer added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding trainer: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTrainer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedTrainer == null)
                {
                    MessageBox.Show("Please select a trainer to update.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _selectedTrainer.Name = txtTrainerName.Text;
                _selectedTrainer.Age = int.Parse(txtTrainerAge.Text);
                _selectedTrainer.Gender = (cmbTrainerGender.SelectedItem as ComboBoxItem)?.Content.ToString();
                _selectedTrainer.Email = txtTrainerEmail.Text;
                _selectedTrainer.Phone = txtTrainerPhone.Text;
                _selectedTrainer.Salary = decimal.Parse(txtTrainerSalary.Text);

                _trainerService.UpdateTrainer(_selectedTrainer);
                LoadTrainers();
                ClearTrainerForm();
                MessageBox.Show("Trainer updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating trainer: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteTrainer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedTrainer == null)
                {
                    MessageBox.Show("Please select a trainer to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _trainerService.DeleteTrainer(_selectedTrainer.TrainerID);
                LoadTrainers();
                ClearTrainerForm();
                MessageBox.Show("Trainer deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting trainer: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgTrainers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedTrainer = dgTrainers.SelectedItem as Trainer;
            if (_selectedTrainer != null)
            {
                txtTrainerName.Text = _selectedTrainer.Name;
                txtTrainerAge.Text = _selectedTrainer.Age.ToString();
                cmbTrainerGender.SelectedItem = cmbTrainerGender.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == _selectedTrainer.Gender);
                txtTrainerEmail.Text = _selectedTrainer.Email;
                txtTrainerPhone.Text = _selectedTrainer.Phone;
                txtTrainerSalary.Text = _selectedTrainer.Salary.ToString();
            }
        }

        private void ClearTrainerForm()
        {
            txtTrainerName.Clear();
            txtTrainerAge.Clear();
            cmbTrainerGender.SelectedIndex = -1;
            txtTrainerEmail.Clear();
            txtTrainerPhone.Clear();
            txtTrainerSalary.Clear();
            _selectedTrainer = null;
        }
    }
}
