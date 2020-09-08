using FestiSpec.Entities.Dal;
using FestiSpec.Maps;
using FestiSpec.Model;
using FestiSpec.ViewModel.Client;
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
using FestiSpec.ViewModel.Schedule;
using FestiSpec.Views.Inspection;
using System;
using FestiSpec.Entity;
using FestiSpec.ViewModel.Employee;
using System.Collections.Generic;

namespace FestiSpec.ViewModel.Festival
{
    public class FestivalListViewModel : ViewModelBase
    {
        public ObservableCollection<FestivalViewModel> Festivals { get; set; }
        public ObservableCollection<RouteData> InspectorEmployees { get; set; }

        private AddFestivalView _addFestivalWindow;

        public bool prevIsMain { get; set; }
        public ICommand AddNewFestivalCommand { get; set; }
        public ICommand OpenFestivalLocation { get; set; }
        public ICommand DeleteFestivalCommand { get; set; }
        public ICommand UpdateFestivalCommand { get; set; }
        public ICommand SetPresenceCommand { get; set; }
        public ICommand UpdateInspectionCommand { get; set; }
        public ICommand DeleteInspectionCommand { get; set; }
        public ICommand CloseInspectionWindowCommand { get; set; }
        public ICommand CheckboxChanged { get; set; }
        public ICommand OpenInspectionViewCommand { get; set; }
        public ICommand OpenAddInspectionViewCommand { get; set; }
        public ICommand AddInspectionCommand { get; set; }

        public ICommand NextPage { get; set; }
        public ICommand PrevPage { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand FilterGridCommand { get; set; }
        private IFestivalRepository _festivalRepository;
        private IInspectionRepository _inspectionRepository;
        private IEmployeeRepository _employeeRepository;

        private bool _festivalSelected = false;

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

        public ObservableCollection<string> comboItems { get; set; }
        private string _selectedComboItem;
        public string SelectedComboItem
        {
            get
            {
                return _selectedComboItem;
            }
            set
            {
                _selectedComboItem = value;
                ConvertFestivals();
                RaisePropertyChanged("SelectedComboItem");
            }
        }

        public int FestivalPage { get; set; }

        public bool FestivalSelected
        {
            get { return _festivalSelected; }
            set
            {
                _festivalSelected = value;
                RaisePropertyChanged("CanEditFestival");
            }
        }

        private bool _canEditFestival = false; // For editable fields

        public bool CanEditFestival
        {
            get { return _canEditFestival; }
            set
            {
                _canEditFestival = value;
                RaisePropertyChanged("CanEditFestival");
            }
        }

        private FestivalViewModel _selectedFestival { get; set; }

        public FestivalViewModel SelectedFestival
        {
            get { return _selectedFestival; }
            set
            {
                _festivalSelected = true;
                _selectedFestival = value;
                ConvertInspections();
                if (_selectedFestival != null) new ViewModelLocator().Questionnaire.LoadQuestionList(_selectedFestival);
                RaisePropertyChanged("SelectedFestival");
                RaisePropertyChanged("FestivalSelected");
            }
        }

        private InspectionViewModel _selectedInspection { get; set; }

        public InspectionViewModel SelectedInspection
        {
            get { return _selectedInspection; }
            set
            {
                _inspectionSelected = true;
                _selectedInspection = value;
                LoadDates();
                RaisePropertyChanged("InspectionSelected");
                RaisePropertyChanged("SelectedInspection");
            }
        }

        private RouteData _selectedEmployee { get; set; }

        public RouteData SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                RaisePropertyChanged("SelectedEmployee");
            }
        }

        public FestivalListViewModel()
        {
            FestivalPage = 0;
            NextPage = new RelayCommand(TurnToNextPage);
            PrevPage = new RelayCommand(TurnToPrevioustPage);
            FilterGridCommand = new RelayCommand(FilterGrid);
            ConvertFestivals(); // Convert from list to observable collection with viewmodels
            ConvertInspections();
            LoadEmployeesThatAreInspectors();

            IsInspectionWindowClosed = true;

            AddNewFestivalCommand = new RelayCommand(ShowAddFestivalView); // ShowAddQuizView
            OpenFestivalLocation = new RelayCommand(ShowLocation);
            DeleteFestivalCommand = new RelayCommand(DeleteSelectedFestival);
            CheckboxChanged = new RelayCommand(CheckBoxChanging);
            UpdateFestivalCommand = new RelayCommand(UpdateSelectedFestival);
            comboItems = new ObservableCollection<string> { "Begin Datum", "Eind Datum", "Bedrijf oplopend", "Bedrijf aflopend" };
            OpenInspectionViewCommand = new RelayCommand(ShowInspectionView);
            UpdateInspectionCommand = new RelayCommand(UpdateInspection);
            SetPresenceCommand = new RelayCommand(SetInspectionsPresence);
            DeleteInspectionCommand = new RelayCommand(DeleteSelectedInspection);
            CloseInspectionWindowCommand = new RelayCommand(InspectionWindowClosed);
            OpenAddInspectionViewCommand = new RelayCommand(ShowAddInspectionView);
            AddInspectionCommand = new RelayCommand(AddNewInspection);
            BackCommand = new RelayCommand(GoBack);
        }

        private void GoBack()
        {
            if (prevIsMain)
            {
                MainMenuView main = new MainMenuView();
                main.Show();
            }
            else
            {
                ClientView client = new ClientView();
                client.Show();
            }
            CloseSelf();

        }
        public void CloseSelf()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is FestivalView)
                {
                    window.Close();
                    break;
                }
            }
        }
        private void FilterGrid()
        {
            ConvertFestivals();
        }


        private void TurnToNextPage()
        {
            FestivalPage += 5;
            ConvertFestivals();
        }

        private void TurnToPrevioustPage()
        {
            FestivalPage -= 5;
            ConvertFestivals();
        }

        private void CheckBoxChanging()
        {
            if (!DBContext.IsOnline) MessageBox.Show("PAS OP, Omdat je in offline mode zit worden je veranderingen NIET opgeslagen!", "Offline Mode");
            if (CanEditFestival)
                CanEditFestival = false;
            else
                CanEditFestival = SelectedFestival != null;
        }

        private void ConvertFestivals()
        {
            _festivalRepository = new FestivalRepository();
            try
            {
                if (_selectedClient != null)
                {
                    var festivalList = _festivalRepository.GetFestivals(_selectedClient.Id).Where(c => c.IsArchived == false).Select(c => new FestivalViewModel(c));
                    Festivals = new ObservableCollection<FestivalViewModel>(festivalList);
                }
                else if (FilterInputString != null)
                {
                    var festivalList = _festivalRepository.GetFestivals().Select(c => new FestivalViewModel(c)).Where(f => f.Name.Contains(FilterInputString) && f.IsArchived == false);

                    if (SelectedComboItem != null)
                    {
                        switch (SelectedComboItem)
                        {
                            case "Begin Datum":
                                Festivals = new ObservableCollection<FestivalViewModel>(festivalList.OrderBy(f => f.StartDate).Skip(FestivalPage).Take(5).ToList());
                                RaisePropertyChanged("Festivals");
                                break;
                            case "Eind Datum":
                                Festivals = new ObservableCollection<FestivalViewModel>(festivalList.OrderByDescending(f => f.EndDate).Skip(FestivalPage).Take(5).ToList());
                                RaisePropertyChanged("Festivals");
                                break;
                            case "Bedrijf asc":
                                Festivals = new ObservableCollection<FestivalViewModel>(festivalList.OrderBy(f => f.Company.NameCompany).Skip(FestivalPage).Take(5).ToList());
                                RaisePropertyChanged("Festivals");
                                break;
                            case "Bedrijf desc":
                                Festivals = new ObservableCollection<FestivalViewModel>(festivalList.OrderByDescending(f => f.Company.NameCompany).Skip(FestivalPage).Take(5).ToList());
                                RaisePropertyChanged("Festivals");
                                break;
                            default:
                                Festivals = new ObservableCollection<FestivalViewModel>(festivalList);
                                RaisePropertyChanged("Festivals");
                                break;
                        }
                    }
                    else
                    {
                        Festivals = new ObservableCollection<FestivalViewModel>(festivalList);
                        RaisePropertyChanged("Festivals");
                    }
                }
                else if (FilterInputString == null && SelectedComboItem != null)
                {
                    var festivalList = _festivalRepository.GetFestivals().Where(c => c.IsArchived == false).Select(c => new FestivalViewModel(c));
                    switch (SelectedComboItem)
                    {
                        case "Begin Datum":
                            Festivals = new ObservableCollection<FestivalViewModel>(festivalList.OrderBy(f => f.StartDate).Skip(FestivalPage).Take(5).ToList());
                            RaisePropertyChanged("Festivals");
                            break;
                        case "Eind Datum":
                            Festivals = new ObservableCollection<FestivalViewModel>(festivalList.OrderByDescending(f => f.EndDate).Skip(FestivalPage).Take(5).ToList());
                            RaisePropertyChanged("Festivals");
                            break;
                        case "Bedrijf asc":
                            Festivals = new ObservableCollection<FestivalViewModel>(festivalList.OrderBy(f => f.Company.NameCompany).Skip(FestivalPage).Take(5).ToList());
                            RaisePropertyChanged("Festivals");
                            break;
                        case "Bedrijf desc":
                            Festivals = new ObservableCollection<FestivalViewModel>(festivalList.OrderByDescending(f => f.Company.NameCompany).Skip(FestivalPage).Take(5).ToList());
                            RaisePropertyChanged("Festivals");
                            break;
                        default:
                            Festivals = new ObservableCollection<FestivalViewModel>(festivalList);
                            RaisePropertyChanged("Festivals");
                            break;
                    }
                }
                else
                {
                    var festivalList = _festivalRepository.GetFestivals().Where(c => c.IsArchived == false).Select(c => new FestivalViewModel(c));
                    var result = festivalList.Skip(FestivalPage).Take(5);
                    Festivals = new ObservableCollection<FestivalViewModel>(result);
                    RaisePropertyChanged("Festivals");
                }
            }
            catch
            {
            }
        }

        public string StartDateString { get; set; }
        public string EndDateString { get; set; }

        public string NewStartDateString { get; set; }
        public string NewEndDateString { get; set; }

        public DateTime NewStartDateDateTime { get; set; }
        public DateTime NewEndDateDateTime { get; set; }

        private ClientViewModel _selectedClient { get; set; }

        public ClientViewModel SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                RaisePropertyChanged("SelectedClient");
                RaisePropertyChanged("HasSelectedClient");
                ConvertFestivals();
                RaisePropertyChanged("Festivals");
            }
        }

        public bool HasSelectedClient
        {
            get { return SelectedClient != null; }
        }

        private void DeleteSelectedFestival()
        {
            if (!DBContext.IsOnline)
            {
                MessageBox.Show("Applicatie is in offline modus.", "Offline Modus");
                return;
            }
            var confirmResult = MessageBox.Show("Bent u zeker dat u " + SelectedFestival.Name.ToString() + " wenst te verwijderen?", "Festival verwijder waarchuwing", MessageBoxButton.YesNo);
            if (confirmResult == MessageBoxResult.Yes)
            {
                SelectedFestival.IsArchived = true;
                RaisePropertyChanged("SelectedFestival");
                SelectedFestival.Update();
                SelectedFestival = null;
                RefreshView();
            }
        }

        //Deletes Selected Inspection
        public void DeleteSelectedInspection()
        {
            if (!DBContext.IsOnline)
            {
                MessageBox.Show("Applicatie is in offline modus.", "Offline Modus");
                return;
            }
            if (SelectedInspection != null)
            {
                var confirmResult = MessageBox.Show("Bent u zeker dat u deze Inspectie wenst te verwijderen?", "Klanten verwijder waarchuwing", MessageBoxButton.YesNo);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    SelectedInspection.Remove();
                    SelectedFestivalInspections.Remove(SelectedInspection);
                    SelectedInspection = null;
                    RaisePropertyChanged("SelectedInspection");
                }
            }
            else
            {
                MessageBox.Show("U heeft geen Inspectie geselecteerd.");
            }
        }
        private QuestionList _selectedQuestionList { get; set; }

        public QuestionList SelectedQuestionList
        {
            get { return _selectedQuestionList; }
            set
            {
                _selectedQuestionList = value;
                RaisePropertyChanged("SelectedQuestionList");
            }
        }

        // Adds a new inspection
        public void AddNewInspection()
        {
            if (SelectedEmployee != null && SelectedQuestionList != null)
            {
                if (ParseNewDateTimeStrings())
                {
                    var tempInspection = new Inspection
                    {
                        StartDate = NewStartDateDateTime,
                        EndDate = NewEndDateDateTime,
                        Employee = SelectedEmployee.Employee,
                        QuestionList = SelectedQuestionList
                    };
                    var context = DBContext.Instance;
                    context.Inspections.Add(tempInspection);
                    context.SaveChanges();

                    SelectedFestivalInspections.Add(new InspectionViewModel(tempInspection));
                    RaisePropertyChanged(() => SelectedFestivalInspections);

                    ViewModelLocator vml = new ViewModelLocator();
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is AddInspectionView)
                        {
                            window.Close();
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("De einddatum moet na de begindatum worden ingevuld.", "Error");
                }
            }
            else
            {
                MessageBox.Show("Niet alle velden zijn correct ingevuld.", "Error");
            }
        }

        private bool ParseNewDateTimeStrings()
        {
            if (DateTime.TryParse(NewStartDateString, out DateTime resultStart) && DateTime.TryParse(NewEndDateString, out DateTime resultEnd))
            {
                if (DateTime.Compare(DateTime.Parse(NewStartDateString), DateTime.Parse(NewEndDateString)) == -1)
                {
                    NewStartDateDateTime = DateTime.Parse(NewStartDateString);
                    NewEndDateDateTime = DateTime.Parse(NewEndDateString);
                    return true;
                }
            }
            return false;
        }

        #region Button Visibility Functionality 
        // These booleans determine if a button should be visible or not.
        private bool _inspectionSelected = false;

        public bool InspectionSelected
        {
            get { return _inspectionSelected; }
            set
            {
                _inspectionSelected = value;
                RaisePropertyChanged("CanEditInspection");
            }
        }

        private bool _canEditInspection = false;

        public bool CanEditClient
        {
            get { return _canEditInspection; }
            set
            {
                _canEditInspection = value;
                RaisePropertyChanged("CanEditInspection");
            }
        }
        #endregion

        private void CheckboxChanging()
        {
            if (!DBContext.IsOnline) MessageBox.Show("PAS OP, Omdat je in offline mode zit worden je veranderingen NIET opgeslagen!", "Offline Mode");
            if (CanEditClient) CanEditClient = false;
            else CanEditClient = SelectedClient != null;
        }

        private void UpdateSelectedFestival()
        {
            SelectedFestival.Update();
        }

        public void RefreshView()
        {
            SelectedFestival = null;
            SelectedInspection = null;
            InspectionSelected = false;
            SelectedQuestionList = null;

            RaisePropertyChanged("SelectedFestival");
            RaisePropertyChanged("SelectedInspection");
            RaisePropertyChanged("SelectedQuestionList");

            ConvertFestivals();
            ConvertInspections();
            LoadEmployeesThatAreInspectors();

            RaisePropertyChanged("InspectionSelected");
            RaisePropertyChanged("Festivals");
            RaisePropertyChanged("CanEditInspection");
        }

        private void ShowLocation()
        {
            if (SelectedFestival != null)
            {
                var temp = new MapsView(SelectedFestival.Model, SelectedFestival.Location, out var success);
                if (success)
                    temp.Show();
                else
                    temp.Close();
            }
            else
            {
                MessageBox.Show("U heeft geen Festival geselecteerd.");
            }
        }

        private void ShowAddFestivalView()
        {
            if (!DBContext.IsOnline)
            {
                MessageBox.Show("Applicatie is in offline modus.", "Offline Modus");
                return;
            }

            _addFestivalWindow = new AddFestivalView(SelectedClient);
            _addFestivalWindow.Show();
        }

        public void InspectionWindowClosed()
        {
            IsInspectionWindowClosed = true;
            RaisePropertyChanged("IsInspectionWindowClosed");
        }

        private void LoadEmployeesThatAreInspectors()
        {
            if (SelectedFestival == null) return;

            InspectorEmployees = new ObservableCollection<RouteData>();

            _employeeRepository = new EmployeeRepository();
            var employeeList = _employeeRepository.GetEmployees().Where(
                e => e.Role.Name == "Inspecteur"
                ).ToList();

            var toRemove = new List<Entity.Employee>();
            foreach (Entity.Employee employee in employeeList)
            {
                List<DateTime> absenceDates = employee.AbsencesDates.Select(x => x.Date.Date).ToList();
                for (DateTime date = SelectedFestival.StartDate.Date; date <= SelectedFestival.EndDate.Date; date = date.AddDays(1))
                {
                    if (absenceDates.Contains(date))
                    {
                        toRemove.Add(employee);
                        break;
                    }
                }
            }

            foreach (Entity.Employee employee in toRemove)
            {
                employeeList.Remove(employee);
            }

            Geodan _geodan = new Geodan("6c4c63db-de9a-11e8-8aac-005056805b87");
            Entity.Festival _festival = SelectedFestival.Model;

            foreach (Entity.Employee employee in employeeList)
            {
                try
                {
                    var responseRoute = _geodan.FindRoute($"&from={employee.City}%20{employee.Street}%20{employee.HouseNumber}&to={_festival.City}%20{_festival.Street}%20{_festival.HouseNumber}&srs=epsg:28992&returntype=coords&outputformat=json");
                    var routeData = new RouteData(employee, (double)responseRoute.features[0].properties.route_distance, (double)responseRoute.features[0].properties.route_duration);
                    InspectorEmployees.Add(routeData);
                }
                catch (NoRouteException)
                {
                    //No valid route was found
                    MessageBox.Show($"Geen route gevonden voor {employee.Name},\nControleer het adres van het festival en de medewerker.", "Geen route gevonden");
                    InspectorEmployees.Add(new RouteData(employee, 999.9, 59940));
                }
            }

            RaisePropertyChanged(() => InspectorEmployees);
        }

        public bool IsInspectionWindowClosed { get; set; }

        public void ShowInspectionView()
        {
            if (!DBContext.IsOnline)
            {
                MessageBox.Show("Applicatie is in offline modus.", "Offline Modus");
                return;
            }
            if (this.SelectedFestival != null && SelectedInspection != null)
            {
                IsInspectionWindowClosed = false;
                RaisePropertyChanged("IsInspectionWindowClosed");
                LoadDates();
                EditInspectionView editInspectionView = new EditInspectionView();
                editInspectionView.Show();
            }
            else
            {
                MessageBox.Show("U heeft geen Inspectie geselecteerd.");
            }
        }

        public void ShowAddInspectionView()
        {
            if (!DBContext.IsOnline)
            {
                MessageBox.Show("Applicatie is in offline modus.", "Offline Modus");
                return;
            }
            if (SelectedFestival != null)
            {
                LoadEmployeesThatAreInspectors();
                AddInspectionView addInspectionView = new AddInspectionView(this.SelectedFestival);
                addInspectionView.Show();
            }
            else
            {
                MessageBox.Show("U heeft geen Festival geselecteerd.");
            }
        }

        private void LoadDates()
        {
            if (SelectedInspection != null)
            {
                this.StartDateString = SelectedInspection.StartDate.ToString();
                this.EndDateString = SelectedInspection.EndDate.ToString();
            }
            else
            {
                this.EndDateString = "";
                this.StartDateString = "";
            }
            RaisePropertyChanged(() => StartDateString);
            RaisePropertyChanged(() => EndDateString);
        }

        private void ConvertInspections()
        {
            if (SelectedFestival != null)
            {
                _inspectionRepository = new InspectionRepository();

                SelectedFestivalInspections = new ObservableCollection<InspectionViewModel>(
                    _inspectionRepository.GetInspections()
                    .Where(e => e.QuestionList.Festival.Id == SelectedFestival.Id)
                    .Select(e => new InspectionViewModel(e))
                    .ToList());
                RaisePropertyChanged("SelectedFestivalInspections");
            }
        }

        private void UpdateInspection()
        {
            if (ParseDateTimeStrings())
            {
                ParseDateTimeStrings();
                SelectedInspection.Update();
                var temp = SelectedInspection;

                SelectedFestivalInspections.Remove(SelectedInspection);
                SelectedFestivalInspections.Add(temp);

                RaisePropertyChanged(() => SelectedInspection);
                RaisePropertyChanged(() => SelectedFestivalInspections);

                ViewModelLocator vml = new ViewModelLocator();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is EditInspectionView)
                    {
                        window.Close();
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("De einddatum moet na de begindatum worden ingevuld.", "Error");
            }

        }

        private bool ParseDateTimeStrings()
        {
            if (DateTime.TryParse(StartDateString, out DateTime resultStart) && DateTime.TryParse(EndDateString, out DateTime resultEnd))
            {
                if (DateTime.Compare(DateTime.Parse(StartDateString), DateTime.Parse(EndDateString)) == -1)
                {
                    SelectedInspection.StartDate = DateTime.Parse(StartDateString);
                    SelectedInspection.EndDate = DateTime.Parse(EndDateString);
                    return true;
                }
            }
            return false;
        }

        private void SetInspectionsPresence()
        {
            if (SelectedInspection != null)
            {
                if (SelectedInspection.Present == false)
                {
                    SelectedInspection.Present = true;
                }
                else
                {
                    SelectedInspection.Present = false;
                }
                RaisePropertyChanged("SelectedInspection");
            }
        }

        public ObservableCollection<InspectionViewModel> SelectedFestivalInspections { get; set; }
    }
}
