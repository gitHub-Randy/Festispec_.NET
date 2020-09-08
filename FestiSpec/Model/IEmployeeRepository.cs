using FestiSpec.Entity;
using System.Collections.Generic;

namespace FestiSpec.Model
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployees();
    }
}