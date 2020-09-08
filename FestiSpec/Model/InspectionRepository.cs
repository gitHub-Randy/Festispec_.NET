using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
using FestiSpec.ViewModel.Schedule;

namespace FestiSpec.Model
{
    public class InspectionRepository : IInspectionRepository
    {
        public List<Inspection> GetInspections()
        {
            
            return DBContext.Instance.Inspections.ToList();
        }

        public Inspection GetInspectionsByDate(DateTime date, int employeeid)
        {
            string convertedDateString = date.ToString("yyyy-MM-dd");
            DateTime convertedDateDateTime = Convert.ToDateTime(convertedDateString);
            foreach(Inspection item in DBContext.Instance.Inspections)
            {
                if (item.StartDate.Date.Equals(convertedDateDateTime.Date) && item.EmployeeId.Equals(employeeid)){
                    return item;
                }
            
              
            }
            return null;
        }
    }
}
