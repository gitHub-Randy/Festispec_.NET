using FestiSpec.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.ViewModel.Schedule
{
    public class ScheduleSlotViewModel
    {
        public Inspection Inspection { get; set; }
        public DateTime Date { get; set; }
        public string ScheduleDate => Date.ToString("dd/MM/yyyy");
        public TimeSpan? Start => Inspection?.StartDate.TimeOfDay;
        public TimeSpan? End => Inspection?.EndDate.TimeOfDay;
        public string Festival => Inspection?.QuestionList?.Festival.Name;

        public bool? Present => Inspection?.Present;
    }
}
