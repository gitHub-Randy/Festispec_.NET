using FestiSpec.Entities.Dal;
using FestiSpec.Model;
using FestiSpec.ViewModel.Festival;
using FestiSpec.Views;
using FestiSpec.Views.Client;
using FestiSpec.Views.Festival;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FestiSpec.ViewModel.Client
{
    public class ClientListViewModel : ViewModelBase
    {
        public ObservableCollection<ClientViewModel> CustomerCompanies { get; set; }
        public ObservableCollection<FestivalViewModel> Festivals { get; set; }

        //List of commands
        public ICommand AddNewClientCommand { get; set; }
        public ICommand DeleteClientCommand { get; set; }
        public ICommand UpdateClientCommand { get; set; }
        public ICommand CheckboxChanged { get; set; }
        public ICommand ShowFestivalCommand { get; set; }

        public ICommand NextPage { get; set; }
        public ICommand PrevPage { get; set; }
        public ICommand FilterGridCommand { get; set; }

        public ICommand BackCommand { get; set; }

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

        public int FestivalPage { get; set; }

       

        public ClientListViewModel()
        {
            ConvertClients(); // Convert from list to observable collection with viewmodels
            AddNewClientCommand = new RelayCommand(ShowAddClientView);
            DeleteClientCommand = new RelayCommand(DeleteSelectedClient);
            UpdateClientCommand = new RelayCommand(UpdateSelectedClient);
            CheckboxChanged = new RelayCommand(CheckboxChanging);
            ShowFestivalCommand = new RelayCommand(ShowFestival);
            FestivalPage = 0;
            NextPage = new RelayCommand(TurnToNextPage);
            PrevPage = new RelayCommand(TurnToPrevioustPage);
            FilterGridCommand = new RelayCommand(FilterGrid);
            BackCommand = new RelayCommand(GoBack);
        }

        private void GoBack()
        {
            MainMenuView main = new MainMenuView();
            main.Show();
            CloseSelf();
        }

        public void CloseSelf()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is ClientView)
                {
                    window.Close();
                    break;
                }
            }
        }

        private void ConvertFestivals()
        {
            _festivalRepository = new FestivalRepository();
            try
            {
                var festivalList = _festivalRepository.GetFestivals(_selectedCustomerCompany.Id).Select(c => new FestivalViewModel(c));
                Festivals = new ObservableCollection<FestivalViewModel>(festivalList);
            }
            catch
            {
            }
        }

        private void FilterGrid()
        {
            ConvertClients();
        }

        private void TurnToNextPage()
        {
            FestivalPage += 5;
            ConvertClients();
        }

        private void TurnToPrevioustPage()
        {
            FestivalPage -= 5;
            ConvertClients();
        }

        private bool _clientSelected = false;

        public bool ClientSelected
        {
            get { return _clientSelected; }
            set
            {
                _clientSelected = value;
                RaisePropertyChanged("CanEditClient");
            }
        }

        private bool _canEditClient = false;

        public bool CanEditClient
        {
            get { return _canEditClient; }
            set
            {
                _canEditClient = value;
                RaisePropertyChanged("CanEditClient");
            }
        }

        private ICustomerCompanyRepository _clientRepository;
        private IFestivalRepository _festivalRepository;
        private ClientViewModel _selectedCustomerCompany { get; set; }

        public ClientViewModel SelectedCustomerCompany
        {
            get { return _selectedCustomerCompany; }
            set
            {
                _clientSelected = true;
                _selectedCustomerCompany = value;
                RaisePropertyChanged("SelectedCustomerCompany");
                RaisePropertyChanged("ClientSelected");
                ConvertFestivals();
                RaisePropertyChanged("Festivals");
            }
        }

        private FestivalViewModel _selectedFestival { get; set; }

        public FestivalViewModel SelectedFestival
        {
            get { return _selectedFestival; }
            set
            {
                _selectedFestival = value;
                RaisePropertyChanged("SelectedFestival");
            }
        }

        public void ShowFestival()
        {
            //TODO: fix only customer related festivals
            FestivalView festivalView = new FestivalView(this.SelectedCustomerCompany);
            festivalView.Show();
            CloseSelf();
        }

        private void CheckboxChanging()
        {
            if (!DBContext.IsOnline) MessageBox.Show("PAS OP, Omdat je in offline mode zit worden je veranderingen NIET opgeslagen!", "Offline Mode");
            if (CanEditClient) CanEditClient = false;
            else CanEditClient = SelectedCustomerCompany != null;
        }

        private void ConvertClients()
        {
            _clientRepository = new CustomerCompanyRepository();
            if(FilterInputString != null)
            {
                var CustomerCompaniesList = _clientRepository.GetCompanies().Where(c => c.IsArchived == false).Select(c => new ClientViewModel(c)).Where(f => f.NameCompany.Contains(FilterInputString)).Skip(FestivalPage).Take(5).ToList();
                CustomerCompanies = new ObservableCollection<ClientViewModel>(CustomerCompaniesList);
                RaisePropertyChanged("CustomerCompanies");
            }
            else
            {
                var CustomerCompaniesList = _clientRepository.GetCompanies().Where(c => c.IsArchived == false).Select(c => new ClientViewModel(c)).Skip(FestivalPage).Take(5).ToList();
                CustomerCompanies = new ObservableCollection<ClientViewModel>(CustomerCompaniesList);
                RaisePropertyChanged("CustomerCompanies");
            }
        }

        private void DeleteSelectedClient()
        {
            if (!DBContext.IsOnline)
            {
                MessageBox.Show("Application is in offline mode.", "Offline Mode");
                return;
            }
            // Prevents application from crashing whenever someone tries to delete a client, when there's none selected. TODO: Hide the buttons when there's no client selected after updates.
            if (_selectedCustomerCompany != null)
            {
                var confirmResult = MessageBox.Show("Bent u zeker dat u " + SelectedCustomerCompany.NameCompany.ToString() + " wenst te verwijderen?", "Klanten verwijder waarchuwing", MessageBoxButton.YesNo);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    SelectedCustomerCompany.IsArchived = true;
                    RaisePropertyChanged("SelectedCustomerCompany");
                    SelectedCustomerCompany.Update();
                    SelectedCustomerCompany = null;
                    SelectedFestival = null;
                    RefreshView();
                }
              
            }
        }

        private void UpdateSelectedClient()
        {
            SelectedCustomerCompany.Update();
        }

        public void RefreshView()
        {
            SelectedCustomerCompany = null;
            RaisePropertyChanged("SelectedCustomerCompany");
            ConvertClients();
            RaisePropertyChanged("CustomerCompanies");
            _clientSelected = false;
            RaisePropertyChanged("ClientSelected");
        }

        private void ShowAddClientView()
        {
            if (!DBContext.IsOnline)
            {
                MessageBox.Show("Application is in offline mode.", "Offline Mode");
                return;
            }
            new AddClientView().Show();
        }
    }
}