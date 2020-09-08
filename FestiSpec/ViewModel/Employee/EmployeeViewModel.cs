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
using System.Linq;
using System.Text;
using System.Windows;

namespace FestiSpec.ViewModel.Employee
{
    public class EmployeeViewModel : ViewModelBase, ICRUD
    {
        public RelayCommand AddEmployeeCommand { get; set; }

        public Entity.Employee Model { get; private set; } //Current Employee

        public string Name { get => Model.Name; set => Model.Name = value; }
        public string Email { get => Model.Email; set => Model.Email = value; }
        public string City { get => Model.City; set => Model.City = value; }
        public string Street { get => Model.Street; set => Model.Street = value; }
        public string HouseNumber { get => Model.HouseNumber; set => Model.HouseNumber = value; }
        public string ZipCode { get => Model.ZipCode; set => Model.ZipCode = value; }

        #region Fields
        public int Id { get { return Model.Id; } }

        public string TelephoneNumber
        {
            get { return Model.TelephoneNumber; }
            set
            {
                if (!value.Equals(""))
                {
                    Model.TelephoneNumber = value;
                }
            }
        }

        public bool IsArchived
        {
            get { return Model.IsArchived; }
            set { Model.IsArchived = value; }
        }

        public Role Role
        {
            get { return Model.Role; }
            set { Model.Role = value; }
        }

        public EmployeeAccount EmployeeAccount
        {
            get { return Model.Account; }
            set { Model.Account = value; }
        }

        public EmployeeViewModel(Entity.Employee employee)
        {
            this.Model = employee;
        }

        [PreferredConstructorAttribute] // Preferred constructor for locator
        public EmployeeViewModel()
        {
            Model = new Entity.Employee(); // Create new Employee when first creating this ViewModel
            EmployeeAccount = new EmployeeAccount();
            AddEmployeeCommand = new RelayCommand(Add);
        }



        #endregion Fields

        #region CRUD

        public void Add()
        {
            try
            {
                var context = DBContext.Instance;
                Model.Role = context.Roles.FirstOrDefault();
                context.Employees.Add(Model);
                context.SaveChanges();

                SetValuesToNull();
                // Close window
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive); // Set current window
                window.Close();

                ViewModelLocator vml = new ViewModelLocator();
                vml.EmployeeListViewModel.RefreshView();
            }
            catch (DbEntityValidationException)
            {
                MessageBox.Show("Niet alle velden zijn correct ingevuld.", "Error");
            }
        }

        private void SetValuesToNull()
        {
            this.Model = new Entity.Employee();
            EmployeeAccount = new EmployeeAccount();
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

        public void Remove()
        {
            var context = DBContext.Instance;
            context.Employees.Remove(Model); // Remove model from the database
            context.SaveChanges();
        }

        #endregion CRUD

        public void View()
        {
            throw new NotImplementedException();
        }
    }
}