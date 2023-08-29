using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmatiKreations.Framework.Utils.Converters
{
    public abstract class BaseConverter : IValueConverter
    {
        public const string InvertParameter = "invert";

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
