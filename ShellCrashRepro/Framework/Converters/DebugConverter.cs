using System.Diagnostics;

namespace EnigmatiKreations.Framework.Utils.Converters
{
    /// <summary>
    /// Converter that does nothing. To use when developing and having trouble to make a complex binding
    /// </summary>
    public class DebugConverter : IValueConverter
    {
        private readonly string DEBUG_CATEGORY = "DEBUGCONVERTER";
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var date = DateTime.Now.ToLongDateString();
            var parameterString = parameter as string;

            if (parameterString == "break")
            {
                Debugger.Break();
            }

            if (value is Element element)
            {
                var p = element.Parent;
                if (element != null && element.BindingContext != null)
                {
                    Debug.WriteLine($"PARENT: {p} - VISUALELEMENT: {element} - BINDINGCONTEXT {element.BindingContext} - DATE: {date}", DEBUG_CATEGORY);
                }
                else
                {
                    Debug.WriteLine($"PARENT: {p} - VALUE: {value} - DATE: {date}", DEBUG_CATEGORY);
                }
            }
            else
            {
                Debug.WriteLine($"VALUE: {value} - DATE: {date}", DEBUG_CATEGORY);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
