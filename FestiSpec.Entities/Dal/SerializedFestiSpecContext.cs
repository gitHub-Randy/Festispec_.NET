using FestiSpec.Entities.Offline;
using FestiSpec.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Entities.Dal
{
    class SerializedFestiSpecContext : IFestiSpecData
    {
        public bool IsOnline => false;
        public SerialData Data { get; set; }

        public SerializedFestiSpecContext()
        {
            Data = SerialData.Load();
            Attachments = new OfflineDbSet<Attachment>(Data.Attachments.ToList());
            ContactPeople = new OfflineDbSet<ContactPerson>(Data.ContactPeople.ToList());
            CustomerCompanies = new OfflineDbSet<CustomerCompany>(Data.CustomerCompanies.ToList());
            Employees = new OfflineDbSet<Employee>(Data.Employees.ToList());
            EmployeeAccounts = new OfflineDbSet<EmployeeAccount>(Data.EmployeeAccounts.ToList());
            Festivals = new OfflineDbSet<Festival>(Data.Festivals.ToList());
            Inspections = new OfflineDbSet<Inspection>(Data.Inspections.ToList());
            PermissionNodes = new OfflineDbSet<PermissionNode>(Data.PermissionNodes.ToList());
            Questions = new OfflineDbSet<Question>(Data.Questions.ToList());
            Answers = new OfflineDbSet<Answer>(Data.Answers.ToList());
            QuestionTypes = new OfflineDbSet<QuestionType>(Data.QuestionTypes.ToList());
            Roles = new OfflineDbSet<Role>(Data.Roles.ToList());
            QuestionLists = new OfflineDbSet<QuestionList>(Data.QuestionLists.ToList());
            EmployeeAbsenceDates = new OfflineDbSet<EmployeeAbsence>(Data.EmployeeAbsenceDates.ToList());
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

        public void SaveChanges()
        {
            //Cannot save :c
        }

        public void Close()
        {
            //Not needed
        }
    }
}
