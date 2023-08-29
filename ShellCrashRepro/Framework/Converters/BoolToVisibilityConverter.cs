using System.Globalization;

namespace EnigmatiKreations.Framework.Utils.Converters
{
    public class BoolToVisibilityConverter : BaseConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var needInvertion = false;
            var stringParameter = parameter as string;
            if (stringParameter == InvertParameter)
            {
                needInvertion = true;
            }
            var isTrue = (bool)value;
            return needInvertion ? !isTrue : isTrue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
