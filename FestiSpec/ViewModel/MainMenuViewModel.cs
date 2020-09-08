using FestiSpec.Entity;
using FestiSpec.Model;
using FestiSpec.Views;
using FestiSpec.Views.Client;
using FestiSpec.Views.Employee;
using FestiSpec.Views.Festival;
using FestiSpec.Views.Schedule;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FestiSpec.ViewModel
{
    public class MainMenuViewModel
    {
        public ICommand LogoutCommand { get; set; }
        public ICommand ShowScheduleCommand { get; set; }
        public ICommand ShowCustomerCommand { get; set; }
        public ICommand ShowEmployeeCommand { get; set; }
        public ICommand ShowFestivalCommand { get; set; }

        public EmployeeAccount Account => EmployeeSession.Instance.User;
        public string Username => Account.Username;
        public string Role { get { return (Account.Employee.Role != null) ? Account.Employee.Role.Name : "Geen"; } }

        public MainMenuViewModel()
        {
            LogoutCommand = new RelayCommand(Logout);
            ShowScheduleCommand = new RelayCommand(ShowSchedule);
            ShowCustomerCommand = new RelayCommand(ShowCustomer);
            ShowEmployeeCommand = new RelayCommand(ShowEmployee);
            ShowFestivalCommand = new RelayCommand(ShowFestival);
        }


        public void Logout()
        {
            LoginView logIn = new LoginView();
            logIn.Show();
            CloseMainMenu();
        }

        public void ShowSchedule()
        {
            ScheduleView scheduleView = new ScheduleView();
            scheduleView.Show();
            CloseMainMenu();
        }

        public void ShowCustomer()
        {
            ClientView clientView = new ClientView();
            clientView.Show();
            CloseMainMenu();


        }

        public void ShowEmployee()
        {
            EmployeeView employeeView = new EmployeeView();
            employeeView.Show();
            CloseMainMenu();

        }
        public void ShowFestival()
        {
            FestivalView festivalView = new FestivalView();
            festivalView.Show();
            CloseMainMenu();

        }

        public void CloseMainMenu()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainMenuView)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
