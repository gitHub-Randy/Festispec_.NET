using FestiSpec.Entities.Offline;
using FestiSpec.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Entities.Dal
{
    public sealed class FestiSpecContext : DbContext, IFestiSpecData
    {
        public bool IsOnline => true;

        public FestiSpecContext() : base("FestiConnection")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        public FestiSpecContext(bool hasProxy) : base("FestiConnection")
        {
            Configuration.ProxyCreationEnabled = hasProxy;
            Configuration.LazyLoadingEnabled = true;
        }

        // DbSets
        public IDbSet<Attachment> Attachments { get; set; }
        public IDbSet<ContactPerson> ContactPeople { get; set; }
        public IDbSet<CustomerCompany> CustomerCompanies { get; set; }
        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<EmployeeAccount> EmployeeAccounts { get; set; }
        public IDbSet<Festival> Festivals { get; set; }
        public IDbSet<Inspection> Inspections { get; set; }
        public IDbSet<PermissionNode> PermissionNodes { get; set; }
        public IDbSet<Question> Questions { get; set; }
        public IDbSet<Answer> Answers { get; set; }
        public IDbSet<QuestionType> QuestionTypes { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<QuestionList> QuestionLists { get; set; }
        public IDbSet<EmployeeAbsence> EmployeeAbsenceDates { get; set; }

        void IFestiSpecData.SaveChanges()
        {
            foreach (var error in GetValidationErrors())
            {
                foreach (var error1 in error.ValidationErrors)
                {
                    error.Entry.Property(error1.PropertyName).IsModified = false;
                }
            }

            SaveChanges();
        }

        public void Close()
        {
            if (DBContext.DoSync)
            {
                SerialData serial = new SerialData(true);
                serial.Save();
            }
            base.Dispose();
        }
    }
}
