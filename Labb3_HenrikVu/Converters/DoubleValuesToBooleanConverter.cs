using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Labb3_HenrikVu.Converters
{
    class DoubleValuesToBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool Boolean = false;
            foreach(var item in values)
            {
                if(item is ICollection collection && collection.Count == 0)
                {
                    return false;
                } 
                else
                {
                    Boolean = true;
                }
            }
            foreach(var item in values)
            {
                if(Boolean == true && item is bool BooleanValue && BooleanValue == false)
                {
                    return false;
                }
            }

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
