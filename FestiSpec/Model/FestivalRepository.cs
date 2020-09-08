using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Model
{
    public class FestivalRepository : IFestivalRepository
    {
        public List<Festival> GetFestivals() // Retrieve list with Festivals
        {
            return DBContext.Instance.Festivals.ToList();
        }

        public List<Festival> GetFestivals(int clientId) // Retrieve list with Festivals by client id
        {
            try
            {
                var CustomerCompany = DBContext.Instance.CustomerCompanies.Single(cc => cc.ContactPerson.Id == clientId);
                return DBContext.Instance.Festivals.Where(f => f.CustomerCompany.Id == CustomerCompany.Id).ToList();
            }
            catch 
            {
                return new List<Festival>();
            }
        }
    }
}
