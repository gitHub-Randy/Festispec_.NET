using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
using FestiSpec.Model;
using FestiSpec.Views;
using FestiSpec.Views.Employee;
using FestiSpec.Views.Schedule;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FestiSpec.ViewModel.Employee
{
    public class EmployeeListViewModel : ViewModelBase
    {
        public ObservableCollection<EmployeeViewModel> Employees { get; set; }
        public ObservableCollection<RoleViewModel> Roles { get; set; }

        private AddEmployeeView _addEmployeeWindow;

        private IEmployeeRepository _employeeRepository;
        public ICommand AddNewEmployeeCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }
        public ICommand UpdateEmployeeCommand { get; set; }
        public ICommand CheckboxChanged { get; set; }
        public ICommand SetRoleCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand FilterGridCommand { get; set; }
        private string _filterInputString;
        public string FilterInputString
        {
            get
            {
                return _filterInputString;
            }
            set
            {
                _filterInputString = value;
                RaisePropertyChanged("FilterInputString");
            }
        }

        public EmployeeListViewModel()
        {
            _employeeRepository = new EmployeeRepository();
            LoadFestivals();

            Roles = new ObservableCollection<RoleViewModel>();
            var tempRoles = DBContext.Instance.Roles.ToList();
            foreach (Role role in tempRoles)
            {
                Roles.Add(new RoleViewModel(role));
            }

            CheckboxChanged = new RelayCommand(CheckBoxChanging);
            AddNewEmployeeCommand = new RelayCommand(ShowAddEmployeeView);
            DeleteEmployeeCommand = new RelayCommand(DeleteSelectedEmployee);
            UpdateEmployeeCommand = new RelayCommand(UpdateSelectedEmployee);
            SetRoleCommand = new RelayCommand(SetRole);
            FilterGridCommand = new RelayCommand(FilterGrid);
            BackCommand = new RelayCommand(GoBack);
        }

        private void GoBack()
        {
            SelectedEmployee = null;
            MainMenuView main = new MainMenuView();
            main.Show();
            CloseMainMenu();
        }

        public void CloseMainMenu()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is EmployeeView || window is ScheduleView)
                {
                    window.Close();
                    break;
                }
            }
        }

        private void FilterGrid()
        {
            LoadFestivals();
        }

        public bool CanShowButtons
        {
            get
            {
                if (CanEditEmployee == true && HasSelectedEmployee)
                {
                    return true;
                }
                return false;
            }
        }

        private EmployeeViewModel _selectedEmployee { get; set; }

        public EmployeeViewModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                if (value != null)
                {
                    HasSelectedEmployee = true;
                }
                else
                {
                    HasSelectedEmployee = false;
                }
                _selectedEmployee = value;
                SetScheduleEmployee();
                SetSelectedRole();
                RaisePropertyChanged("SelectedEmployee");
                RaisePropertyChanged("HasSelectedEmployee");
                RaisePropertyChanged("CanShowButtons");
                RaisePropertyChanged("Username");
                RaisePropertyChanged("Password");
                RaisePropertyChanged("CanShowTextField");
            }
        }

        public void SetScheduleEmployee()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is ScheduleView && SelectedEmployee != null)
                {
                    ViewModelLocator vml = new ViewModelLocator();
                    vml.InspectionListViewModel.EmployeeID = SelectedEmployee.Id;
                    break;
                }
            }
        }

        private bool _hasSelectedEmployee = false;

        public bool HasSelectedEmployee
        {
            get { return _hasSelectedEmployee; }
            set
            {
                _hasSelectedEmployee = value;
                RaisePropertyChanged("HasSelectedEmployee");
            }
        }

        private bool _canEditEmployee = false; // For editable fields
        
        public bool CanEditEmployee
        {
            get { return _canEditEmployee; }
            set
            {
                _canEditEmployee = value;
                RaisePropertyChanged("CanEditEmployee");
                RaisePropertyChanged("CanShowButtons");
            }
        }

        private RoleViewModel _selectedRole { get; set; }

        public RoleViewModel SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                RaisePropertyChanged("SelectedRole");
            }
        }

        private void CheckBoxChanging()
        {
            if (CanEditEmployee)
                CanEditEmployee = false;
            else
                CanEditEmployee = true;
            RaisePropertyChanged("CanShowTextField");
        }

        public string Username
        {
            get
            {
                if (SelectedEmployee != null)
                {
                    return SelectedEmployee.EmployeeAccount.Username;
                }
                return null;
            }
            set
            {
                if (!value.Equals(""))
                {
                    SelectedEmployee.EmployeeAccount.Username = value;
                }
                else
                {
                    //TODO throw error/message
                }
            }
        }

        public string Password
        {
            get
            {
                if (SelectedEmployee != null)
                {
                    return SelectedEmployee.EmployeeAccount.Password;
                }
                return null;
            }
            set
            {
                if (!value.Equals(""))
                {
                    SelectedEmployee.EmployeeAccount.Password = value;
                }
                else
                {
                    //TODO throw error/message
                }
            }
        }

        public void RefreshView()
        {
            SelectedEmployee = null;
            RaisePropertyChanged("SelectedEmployee");
            LoadFestivals();
            RaisePropertyChanged("Employees");
        }

        private void LoadFestivals()
        {
            if (FilterInputString != null)
            {
                var employeeList = _employeeRepository.GetEmployees().Where(c => c.IsArchived == false).Select(c => new EmployeeViewModel(c)).Where(f => f.Name.Contains(FilterInputString)).ToList(); ;
                Employees = new ObservableCollection<EmployeeViewModel>(employeeList);
                RaisePropertyChanged("Employees");
            }
            else
            {
                var employeeList = _employeeRepository.GetEmployees().Where(c => c.IsArchived == false).Select(c => new EmployeeViewModel(c));
                Employees = new ObservableCollection<EmployeeViewModel>(employeeList);
                RaisePropertyChanged("Employees");
            }
        }

        private void UpdateSelectedEmployee()
        {
            SelectedEmployee.Update();
        }

        private void DeleteSelectedEmployee()
        {
            var confirmResult = MessageBox.Show("Bent u zeker dat u " + SelectedEmployee.Name.ToString() + " wenst te verwijderen?", "Medewerker verwijder waarchuwing", MessageBoxButton.YesNo);
            if (confirmResult == MessageBoxResult.Yes)
            {
                SelectedEmployee.IsArchived = true;
                RaisePropertyChanged("SelectedEmployee");
                SelectedEmployee.Update();
                SelectedEmployee = null;
                RefreshView();
            }
       
        }

        public bool CanShowTextField
        {
            get { return SelectedEmployee != null && CanEditEmployee == true; }
        }

        private void ShowAddEmployeeView()
        {
            EmployeeViewModel add = new EmployeeViewModel();

            _addEmployeeWindow = new AddEmployeeView();
            _addEmployeeWindow.Show();
        }

        private void SetSelectedRole()
        {
            if (SelectedEmployee != null && SelectedEmployee != null)
            {
                SelectedRole = Roles.Single(x => x.Id == SelectedEmployee.Role.Id);
            }
        }

        private void SetRole()
        {
            if (SelectedRole != null && SelectedEmployee != null)
            {
                SelectedEmployee.Role = DBContext.Instance.Roles.Single(e => e.Id == SelectedRole.Id);
                SelectedEmployee.Update();
                RaisePropertyChanged("SelectedEmployee");
            }
        }
    }
}
