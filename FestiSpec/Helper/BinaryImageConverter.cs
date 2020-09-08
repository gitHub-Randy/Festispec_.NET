using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace FestiSpec.Helper
{
    public class BinaryImageConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;

            if (s == null)
                return null;

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.StreamSource = new MemoryStream(System.Convert.FromBase64String(value.ToString()));
            bi.EndInit();

            return bi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
