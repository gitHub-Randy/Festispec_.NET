using FestiSpec.ViewModel.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FestiSpec.Views.Schedule
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            InitializeComponent();
            ((InspectionListViewModel)DataContext).FillSchedule();
        }
        public SchedulePage(int employeeid)
        {
            InitializeComponent();
            ((InspectionListViewModel)DataContext).EmployeeID = employeeid;
            ((InspectionListViewModel)DataContext).FillSchedule();

        }
    }
}
