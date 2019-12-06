using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DominantColoursSearch.Converters
{
    public class BackgroungToForegroundCalculation : IValueConverter
    {
        /// <summary>
        /// Calculate proper foreground color depending on a background
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Luma_%28video%29#Use_of_relative_luminance
        /// </remarks>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SolidColorBrush colorBrush))
            {
                return DependencyProperty.UnsetValue;
            }

            Color color = colorBrush.Color;

            double luminance = 0.2126 * color.ScR +
                0.7152 * color.ScG +
                0.0722 * color.ScB;

            return luminance < 0.5d ? Brushes.White : Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
