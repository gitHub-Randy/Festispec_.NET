using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FestiSpec.ViewModel.Schedule
{
    class PresentStringColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ScheduleSlotViewModel slot = (value as ScheduleSlotViewModel);
            if (slot.Present == false && slot.Start != null && slot.End !=null && slot.Festival != null) return new SolidColorBrush(Colors.IndianRed);
            else if (slot.Present == true && slot.Start != null && slot.End != null && slot.Festival != null) return new SolidColorBrush(Colors.LightSeaGreen);
            else return new SolidColorBrush(Colors.Wheat); 
        
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
