using System;
using System.Collections.Generic;
using FestiSpec.Entity;
using FestiSpec.ViewModel.Schedule;

namespace FestiSpec.Model
{
    public interface IInspectionRepository
    {
        List<Inspection> GetInspections();
        Inspection GetInspectionsByDate(DateTime datetime, int employeeid);
    }
}
