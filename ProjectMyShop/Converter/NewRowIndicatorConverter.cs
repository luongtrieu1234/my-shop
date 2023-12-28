using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectMyShop.Converter
{
    public class NewRowIndicatorConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "";
            if (value.GetType().FullName == "MS.Internal.NamedObject")
                return "*";
            return "";
        }
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // this function is not supposed to be called
            throw new Exception();
        }
    }
}
