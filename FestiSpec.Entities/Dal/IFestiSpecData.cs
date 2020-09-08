using FestiSpec.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Entities.Dal
{
    public interface IFestiSpecData
    {
        bool IsOnline { get; }
        IDbSet<Attachment> Attachments { get; set; }
        IDbSet<ContactPerson> ContactPeople { get; set; }
        IDbSet<CustomerCompany> CustomerCompanies { get; set; }
        IDbSet<Employee> Employees { get; set; }
        IDbSet<EmployeeAccount> EmployeeAccounts { get; set; }
        IDbSet<Festival> Festivals { get; set; }
        IDbSet<Inspection> Inspections { get; set; }
        IDbSet<PermissionNode> PermissionNodes { get; set; }
        IDbSet<Question> Questions { get; set; }
        IDbSet<Answer> Answers { get; set; }
        IDbSet<QuestionType> QuestionTypes { get; set; }
        IDbSet<Role> Roles { get; set; }
        IDbSet<QuestionList> QuestionLists { get; set; }
        IDbSet<EmployeeAbsence> EmployeeAbsenceDates { get; set; }

        void SaveChanges();
        void Close();
    }
}
