using FestiSpec.Entity;
using FestiSpec.ViewModel.Questionnaire.VM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FestiSpec.Helper
{
    class ItemTemplateSelector : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is QuestionTypeVM s)
            {
                var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                var url = $"{assemblyName}.Views.Question.Control.{TypeConverter.type[s.Type]}";
                Type userControl = Type.GetType(url, null, null);
                return Activator.CreateInstance(userControl ?? Type.GetType($"{assemblyName}.Views.Question.Control.EmptyControl"));
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
