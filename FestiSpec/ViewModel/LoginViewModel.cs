using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
using FestiSpec.Model;
using FestiSpec.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FestiSpec.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private MainMenuView mainMenu;

        public RelayCommand<object> _passwordCommand { get; private set; }

        public string Username
        {
            get { return this._username; }
            set { this._username = value; }
        }



        public LoginViewModel()
        {
            _passwordCommand = new RelayCommand<object>(LogIn);
        }

        public void LogIn(object parameter)
        {
            string password = ((PasswordBox)parameter).Password.ToString();
            EmployeeAccount account = MatchCredentials(Username, password);
            if (account != null)
            {
                EmployeeSession.Instance.User = account;
                mainMenu = new MainMenuView();
                mainMenu.Show();
                CloseLogin();
            }
            else
            {
                MessageBox.Show("Verkeerde inloggegevens of gebruiker bestaat niet.");
            }
        }

        public EmployeeAccount MatchCredentials(string username, string password)
        {
            try
            {
                if (username == null || password == null) return null;
                foreach (EmployeeAccount account in DBContext.Instance.EmployeeAccounts.ToList())
                {
                    if (account.Username.Equals(username) && account.Password.Equals(password))
                    {
                        if (!account.Employee.IsArchived)
                        {
                            return account;
                        }
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Er bestaat nog geen sync data.\nStart de applicatie eerst in online modus op.", "Fout");
                Environment.Exit(0);
            }
            return null;
        }

        public void CloseLogin()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is LoginView)
                {
                    window.Close();
                    break;
                }
            }
        }

    }
}
