
using System.Globalization;

namespace EnigmatiKreations.Framework.Utils.Converters
{
    public class ItemTappedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ItemTappedEventArgs eventArgs)
                throw new ArgumentException("Expected TappedEventArgs as value", nameof(value));

            return eventArgs.Item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
