using CommonServiceLocator;
using FestiSpec.ViewModel;
using FestiSpec.ViewModel.Client;
using FestiSpec.ViewModel.Employee;
using FestiSpec.ViewModel.Festival;
using FestiSpec.ViewModel.Questionnaire;
using FestiSpec.ViewModel.Schedule;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace FestiSpec.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<QuestionnaireViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<MainMenuViewModel>();
            SimpleIoc.Default.Register<ClientViewModel>();
            SimpleIoc.Default.Register<ClientListViewModel>();
            SimpleIoc.Default.Register<FestivalViewModel>();
            SimpleIoc.Default.Register<FestivalListViewModel>();
            SimpleIoc.Default.Register<EmployeeViewModel>();
            SimpleIoc.Default.Register<EmployeeListViewModel>();
            SimpleIoc.Default.Register<InspectionViewModel>();
            SimpleIoc.Default.Register<InspectionListViewModel>();
            SimpleIoc.Default.Register<OptionHandlerViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public MainMenuViewModel MainMenu
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainMenuViewModel>();
            }
        }
        public QuestionnaireViewModel Questionnaire
        {
            get
            {
                return ServiceLocator.Current.GetInstance<QuestionnaireViewModel>();
            }
        }

        public ClientListViewModel ClientListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ClientListViewModel>();
            }
        }

        public ClientViewModel ClientViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ClientViewModel>();
            }
        }

        public ClientViewModel AddClientViewModel
        {
            get
            {
                return new ClientViewModel();
            }
        }

        public FestivalViewModel FestivalViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FestivalViewModel>();
            }
        }

        public FestivalListViewModel FestivalListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FestivalListViewModel>();
            }
        }

        public LoginViewModel Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }

        public OptionHandlerViewModel OptionHandlerViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<OptionHandlerViewModel>();
            }
        }

        public EmployeeViewModel EmployeeViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EmployeeViewModel>();
            }
        }

        public EmployeeListViewModel EmployeeListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EmployeeListViewModel>();
            }
        }

        public InspectionViewModel InspectionViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InspectionViewModel>();
            }
        }

        public InspectionListViewModel InspectionListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InspectionListViewModel>();
            }
        }


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}