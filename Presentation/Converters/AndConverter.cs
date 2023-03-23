using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ProductCatalogue.WPF.Presentation.Converters
{
    public class AndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.All(o => o is bool condition && condition);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
