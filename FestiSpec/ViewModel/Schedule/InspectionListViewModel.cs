using FestiSpec.Model;
using FestiSpec.ViewModel.Festival;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace FestiSpec.ViewModel.Schedule
{
    public class InspectionListViewModel : ViewModelBase
    {
        public ScheduleSlotViewModel[] WeekSlots { get; set; }
        private int employeeid;
        public ICommand NextWeekCommand { get; set; }
        public ICommand PreviousWeekCommand { get; set; }
        public ICommand BackToCurrentWeek { get; set; }
        private int currentWeek;
        public DateTime CurrentDate { get; set; }
        private IInspectionRepository _inspectionRepository;

     
        public InspectionListViewModel()
        {
            CurrentDate = GetCurrentMonday(DayOfWeek.Monday);
            CurrentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(CurrentDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            _inspectionRepository = new InspectionRepository();
            NextWeekCommand = new RelayCommand(AddSevenDays);
            PreviousWeekCommand = new RelayCommand(SubtractSevenDays);
            WeekSlots = new ScheduleSlotViewModel[7];
            FillSchedule();
            BackToCurrentWeek = new RelayCommand(ToCurrentWeek);
        }

        public void ToCurrentWeek()
        {
            CurrentDate = GetCurrentMonday(DayOfWeek.Monday);
            var day = CurrentDate;
            string currentDay = day.ToShortDateString();
            CurrentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(CurrentDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            FillSchedule();

        }

        public int EmployeeID
        {
            get
            {
                return employeeid;
            }
            set
            {
                this.employeeid = value;
                RaisePropertyChanged("EmployeeID");
                FillSchedule();
            }
        }


        public int CurrentWeek
        {
            get
            {
                return currentWeek;
            }
            set
            {
                currentWeek = value;
                RaisePropertyChanged(() => CurrentWeek);
            }
        }


        public void FillSchedule()
        {
            DateTime loopDate = new DateTime(CurrentDate.Ticks);
            for (int i = 0; i < WeekSlots.Length; i++)
            {
                WeekSlots[i] = new ScheduleSlotViewModel();
                WeekSlots[i].Inspection = _inspectionRepository.GetInspectionsByDate(loopDate.Date, employeeid);
                WeekSlots[i].Date = loopDate;
                loopDate = loopDate.AddDays(1);
            }
            RaisePropertyChanged(() => WeekSlots);
        }

        public DateTime GetCurrentMonday(DayOfWeek day)
        {
            if (DateTime.Now.DayOfWeek != day)
            {
                DateTime result = DateTime.Now.AddDays(-1);
                while (result.DayOfWeek != day)
                    result = result.AddDays(-1);
                return result;
            }
            return DateTime.Now;
        }

        private void AddSevenDays()
        {
            CurrentDate = CurrentDate.AddDays(7);
            CurrentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(CurrentDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            FillSchedule();
        }

        private void SubtractSevenDays()
        {
            CurrentDate = CurrentDate.AddDays(-7);
            CurrentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(CurrentDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            FillSchedule();
        }

    }
}
