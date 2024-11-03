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
    class DoubleBooleanHandlerConverter2 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach(var item in values)
            {
                if(item is bool booleanValue && !booleanValue)
                {
                    return false;
                }
                if(item is ICollection collection && collection.Count != 0)
                {
                    if(collection.Count != 0)
                    {
                        return true;
                    } 
                    else
                    {
                        return false;
                    }
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