using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmatiKreations.Framework.Utils.Converters
{
    /// <summary>
    /// A converter that allows to convert any object to a boolean (for visibility purposes)
    /// </summary>
    /// <remarks>
    /// Invert mode: makes that null or empty objects are not visible
    /// Normal mode: makes that null or empty objects are visible
    /// </remarks>
    public class NullOrEmptyToVisibilityConverter : BaseConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var needInvertion = false;
            var stringParameter = parameter as string;
            if (stringParameter == InvertParameter)
            {
                needInvertion = true;
            }

            if(value is int count)
            {
                return needInvertion ? count != 0 : count == 0;
            }
            if (value is DateTime date)
            {
                return needInvertion ? date != DateTime.MinValue : date == DateTime.MinValue;
            }

            if(value is string st)
            {
                return needInvertion ? string.IsNullOrWhiteSpace(st) : !string.IsNullOrWhiteSpace(st);
            }

            if(value is IList list)
            {
                return needInvertion ? list == null || list.Count == 0 : list != null && list.Count != 0;
            }

            return needInvertion ? value == null : value != null;
        }


        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
