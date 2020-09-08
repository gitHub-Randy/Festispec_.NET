using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
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
using System.Windows;
using System.Windows.Input;

namespace FestiSpec.ViewModel.Client
{
    public class ClientViewModel : ViewModelBase, ICRUD
    {
        public CustomerCompany Model { get; private set; } // Current Client
        public ICommand AddClientCommand { get; set; }

        public int Id { get { return Model.Id; } }

        // Comp Name Property
        public string NameCompany
        {
            get { return Model.NameCompany; }
            set { Model.NameCompany = value; }
        }

        public string City { get => Model.City; set => Model.City = value; }
        public string Street { get => Model.Street; set => Model.Street = value; }
        public string HouseNumber { get => Model.HouseNumber; set => Model.HouseNumber = value; }
        public string ZipCode { get => Model.ZipCode; set => Model.ZipCode = value; }

        public bool IsArchived
        {
            get { return Model.IsArchived; }
            set { Model.IsArchived = value; }
        }

        public ContactPerson ContactPerson
        {
            get { return Model.ContactPerson; }
            set { Model.ContactPerson = value; }
        }

        public ClientViewModel(CustomerCompany CustomerCompany)
        {
            this.Model = CustomerCompany;
            this.Model.ContactPerson = ContactPerson;
        }

        [PreferredConstructorAttribute] // Preferred constructor for locator
        public ClientViewModel()
        {
            Model = new CustomerCompany(); // Create new Client when first creating this ViewModel
            ContactPerson = new ContactPerson();
            AddClientCommand = new RelayCommand(Add);
        }

        #region CRUD
        public void Add()
        {
            try
            {
                var context = DBContext.Instance;
                context.CustomerCompanies.Add(Model);
                context.SaveChanges();

                // Close add window
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive); // Set current window
                window.Close(); // Close current window

                ViewModelLocator vml = new ViewModelLocator();
                vml.ClientListViewModel.RefreshView();
            }
            catch (DbEntityValidationException)
            {
                MessageBox.Show("Niet alle velden zijn correct ingevuld.", "Error");
            }
        }

        public void Remove()
        {
            var context = DBContext.Instance;
            context.CustomerCompanies.Remove(Model);
            context.SaveChanges();
        }

        public void Update()
        {
            try
            {
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
        #endregion CRUD

        public void View()
        {
            throw new NotImplementedException();
        }
    }
}
