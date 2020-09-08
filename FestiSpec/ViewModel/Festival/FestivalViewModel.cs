using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
using FestiSpec.ViewModel;
using FestiSpec.ViewModel.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace FestiSpec.ViewModel.Festival
{
    public class FestivalViewModel : ViewModelBase, ICRUD
    {
        private CustomerCompanyViewModel _customerCompanyViewModel = new CustomerCompanyViewModel();

        public string City { get => Model.City; set => Model.City = value; }
        public string Street { get => Model.Street; set => Model.Street = value; }
        public string HouseNumber { get => Model.HouseNumber; set => Model.HouseNumber = value; }
        public string Comment { get => Model.LocationComment; set => Model.LocationComment = value; }

        public string Location
        {
            get
            {
                return $"{City} {Street} {HouseNumber}";
            }
        }

        public bool IsArchived
        {
            get { return Model.IsArchived; }
            set { Model.IsArchived = value; }
        }

        public Entity.Festival Model { get; private set; } // Current Festival

        public ICommand AddFestivalCommand { get; set; }

        #region Fields
        public int Id { get { return Model.Id; } }

        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; }
        }

        public DateTime StartDate
        {
            get { return Model.StartDate; }
            set { Model.StartDate = value; }
        }

        public DateTime EndDate
        {
            get { return Model.EndDate; }
            set { Model.EndDate = value; }
        }

        public CustomerCompany Company
        {
            get { return Model.CustomerCompany; }
            set { Model.CustomerCompany = value; }
        }

        public virtual ICollection<QuestionList> QuestionLists
        {
            get { return Model.QuestionLists ?? (Model.QuestionLists = new List<QuestionList>()); }
            set { Model.QuestionLists = value; }
        }

        public FestivalViewModel(Entity.Festival festival)
        {
            this.Model = festival;
        }

        [PreferredConstructorAttribute] // Preferred constructor for locator
        public FestivalViewModel()
        {
            Model = new Entity.Festival(); // Create new Festival when first creating this ViewModel
            SetDates(); // Set dates for selection
            AddFestivalCommand = new RelayCommand(Add);
        }

        private void SetDates()
        {
            Model.StartDate = DateTime.Today;
            Model.EndDate = DateTime.Today;
        }


        #endregion Fields

        #region CRUD

        public void Add()
        {
            try
            {
                if(Model.StartDate < DateTime.Now.Date)
                {
                    MessageBox.Show("De startdatum kan niet voor vandaag ingevuld worden.", "Foute informatie ingevuld");
                    return;
                }
                if (Model.StartDate > Model.EndDate)
                {
                    MessageBox.Show("De startdatum valt na de einddatum", "Foute informatie ingevoerd.");
                    return;
                }

                var context = DBContext.Instance;
                context.Festivals.Add(Model);
                context.SaveChanges();

                // Close add window
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive); // Set current window
                window.Close(); // Close current window

                ViewModelLocator vml = new ViewModelLocator();
                vml.FestivalListViewModel.RefreshView();
            }
            catch (DbEntityValidationException)
            {
                MessageBox.Show("Niet alle velden zijn correct ingevuld.", "Error");
            }
        }

        public void Update()
        {
            try
            {
                if (Model.StartDate > Model.EndDate)
                {
                    MessageBox.Show("De startdatum valt na de einddatum", "Foute informatie ingevoerd.");
                    return;
                }
                var context = DBContext.Instance;
                context.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
            }

            if (DBContext.Instance is DbContext)
            {
                (DBContext.Instance as DbContext).Entry(Model).Reload();
                RaisePropertyChanged(string.Empty);
            }
        }

        public void Remove()
        {
            var context = DBContext.Instance;
            context.Festivals.Remove(Model); // Remove model from the database
            context.SaveChanges();
        }

        #endregion CRUD

        public void View()
        {
            throw new NotImplementedException();
        }
    }
}