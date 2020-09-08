using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Entities.Offline
{
    [Serializable]
    class SerialData
    {
        public SerialData(bool load)
        {
            if (!load) return;

            FestiSpecContext data = new FestiSpecContext(false);

            Attachments = data.Attachments.ToList();

            foreach(Attachment attachment in Attachments)
            {
                attachment.FilePath = " ";
            }

            ContactPeople = data.ContactPeople.ToList();
            CustomerCompanies = data.CustomerCompanies.ToList();
            Employees = data.Employees.ToList();
            EmployeeAccounts = data.EmployeeAccounts.ToList();
            Festivals = data.Festivals.ToList();
            Inspections = data.Inspections.ToList();
            PermissionNodes = data.PermissionNodes.ToList();
            Questions = data.Questions.ToList();
            Answers = data.Answers.ToList();
            QuestionTypes = data.QuestionTypes.ToList();
            Roles = data.Roles.ToList();
            QuestionLists = data.QuestionLists.ToList();
            EmployeeAbsenceDates = data.EmployeeAbsenceDates.ToList();
        }

        private SerialData()
        {
            //Used for loading
        }

        public IList<Attachment> Attachments { get; set; }
        public IList<ContactPerson> ContactPeople { get; set; }
        public IList<CustomerCompany> CustomerCompanies { get; set; }
        public IList<Employee> Employees { get; set; }
        public IList<EmployeeAccount> EmployeeAccounts { get; set; }
        public IList<Festival> Festivals { get; set; }
        public IList<Inspection> Inspections { get; set; }
        public IList<PermissionNode> PermissionNodes { get; set; }
        public IList<Question> Questions { get; set; }
        public IList<Answer> Answers { get; set; }
        public IList<QuestionType> QuestionTypes { get; set; }
        public IList<Role> Roles { get; set; }
        public IList<QuestionList> QuestionLists { get; set; }
        public IList<EmployeeAbsence> EmployeeAbsenceDates { get; set; }

        public void Save()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("sync.dat", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, this);
            stream.Close();
        }

        public static SerialData Load()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("sync.dat", FileMode.Open, FileAccess.Read);
            SerialData newcontext = (SerialData)formatter.Deserialize(stream);
            stream.Close();
            return newcontext;
        }
    }
}
