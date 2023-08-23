using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FP_Task_two.Converters
{
    internal class BoolToIsConnectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isConnected)
            {
                return isConnected ? "Закрыть передачу по UDP" : "Открыть передачу по UDP";
            }
            return "Error: BTIC_C";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return str == "Закрыть передачу по UDP";
            }
            return false;
        }
    }
}
