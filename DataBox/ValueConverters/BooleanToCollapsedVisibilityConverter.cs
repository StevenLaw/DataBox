using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DataBox.ValueConverters
{
    /// <summary>
    /// Returns 0.25 opacity style if parameter is false and 1.0 if true.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    /// <remarks>
    /// To be set on is enabled
    /// </remarks>
    public class BooleanToGreyedOpacityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //reverse conversion (false=>0.25 opacity, true=>1.0 opacity) on any given parameter
            bool input = (null == parameter) ? (bool)value : !((bool)value);
            Style style = new Style(typeof(Image));
            style.Setters.Add(new Setter(Image.OpacityProperty, (input) ? 1.0 : 0.25));
            return style;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
