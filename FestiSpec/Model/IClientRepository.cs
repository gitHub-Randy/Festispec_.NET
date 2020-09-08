using FestiSpec.Entity;
using System.Collections.Generic;

namespace FestiSpec.Model
{
    public interface ICustomerCompanyRepository
    {
        List<CustomerCompany> GetCompanies();
    }
}
