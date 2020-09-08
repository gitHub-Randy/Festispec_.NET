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
    public class CustomerCompanyRepository : ICustomerCompanyRepository
    {
        // Retrieve List with CustomerCompanies
        public List<CustomerCompany> GetCompanies()
        {
            return DBContext.Instance.CustomerCompanies.ToList();
        }
    }
}
