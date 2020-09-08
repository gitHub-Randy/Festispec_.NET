using FestiSpec.ViewModel.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.ViewModel
{
    public class RoleViewModel : ICRUD
    {
        public Entity.Role Model { get; private set; } //Current Role

        #region Fields
        public int Id { get { return Model.Id; } }

        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; }
        }

        public RoleViewModel(Entity.Role role)
        {
            this.Model = role;
        }

        [PreferredConstructorAttribute] // Preferred constructor for locator
        public RoleViewModel()
        {
            Model = new Entity.Role(); // Create new Role when first creating this ViewModel
            //AddEmployeeCommand = new RelayCommand(Add);
        }

        #endregion Fields
        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void View()
        {
            throw new NotImplementedException();
        }
    }
}
