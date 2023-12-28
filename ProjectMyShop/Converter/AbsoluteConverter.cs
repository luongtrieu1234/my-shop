using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectMyShop.Converter
{
    class AbsoluteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string relativePath = (string)value;
            string folder = AppDomain.CurrentDomain.BaseDirectory; // C:\Folder\    
            string absolutePath = $"{folder}{relativePath}";
            Console.WriteLine("folder:"+ folder);
            Console.WriteLine("relativePath"+relativePath);
            return absolutePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
