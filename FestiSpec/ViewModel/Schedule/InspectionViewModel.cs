using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
using FestiSpec.ViewModel.Festival;
using FestiSpec.ViewModel.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FestiSpec.ViewModel.Schedule
{
    public class InspectionViewModel : ICRUD
    {
        public Inspection Model { get; private set; }

        [PreferredConstructorAttribute] // Preferred constructor for locator
        public InspectionViewModel()
        {
            Model = new Inspection(); // Create new Inspection when first creating this ViewModel
        }

        public InspectionViewModel(Inspection inspection) // Constuctor for inspection only
        {
            this.Model = inspection;
        }

        public InspectionViewModel(Inspection inspection, DateTime date) // Constructor for inspection and date
        {
            this.Model = inspection;
            this.Date = date;
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

        public bool Present
        {
            get { return Model.Present; }
            set { Model.Present = value; }
        }

        public string ReasonsAbsent
        {
            get { return Model.ReasonsAbsent; }
            set { Model.ReasonsAbsent = value; }
        }

        public void SetModel(Inspection model)
        {
            this.Model = model;
        }

        private DateTime _date { get; set; }

        public DateTime Date
        {
            get { return _date; }
            set { this._date = value; }
        }

        #region CRUD
        public void Add()
        {
            
            if (Model.StartDate != null && Model.EndDate != null && Model.Employee != null)
            {
                throw new NotImplementedException(); //TODO Add inspection
            }
        }

        public void Remove()
        {
            var context = DBContext.Instance;
            context.Inspections.Remove(Model); // Remove model from the database
            context.SaveChanges();
        }

        public void Update()
        {
            if (Model.StartDate != null && Model.EndDate != null && Model.Employee != null)
            {
                var context = DBContext.Instance;
                context.SaveChanges();
            }
        }

        public void View()
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}