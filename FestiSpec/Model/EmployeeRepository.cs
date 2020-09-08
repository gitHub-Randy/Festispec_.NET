using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
using FestiSpec.ViewModel;

namespace FestiSpec.Model
{
    public class EmployeeRepository : IEmployeeRepository
    {
        // Retrieve List with Employees
        public List<Employee> GetEmployees()
        {
            return DBContext.Instance.Employees.ToList();
        }
    }
}